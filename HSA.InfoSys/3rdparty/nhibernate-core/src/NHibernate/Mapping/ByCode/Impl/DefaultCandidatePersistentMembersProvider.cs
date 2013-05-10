using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NHibernate.Mapping.ByCode.Impl
{
	public class DefaultCandidatePersistentMembersProvider : ICandidatePersistentMembersProvider
	{
		internal const BindingFlags SubClassPropertiesBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
		internal const BindingFlags RootClassPropertiesBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
		internal const BindingFlags ComponentPropertiesBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

		internal const BindingFlags ClassFieldsBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

		#region Implementation of ICandidatePersistentMembersProvider

		public IEnumerable<MemberInfo> GetEntityMembersForPoid(System.Type entityClass)
		{
			return entityClass.IsInterface
					? entityClass.GetInterfaceProperties()
					: entityClass.GetPropertiesOfHierarchy().Concat(GetFieldsOfHierarchy(entityClass));
		}

		public IEnumerable<MemberInfo> GetRootEntityMembers(System.Type entityClass)
		{
			return GetCandidatePersistentProperties(entityClass, RootClassPropertiesBindingFlags).Concat(GetUserDeclaredFields(entityClass).Cast<MemberInfo>());
		}

		public IEnumerable<MemberInfo> GetSubEntityMembers(System.Type entityClass, System.Type entitySuperclass)
		{
			const BindingFlags flattenHierarchyBindingFlag =
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

			if (!entitySuperclass.Equals(entityClass.BaseType))
			{
				IEnumerable<MemberInfo> propertiesOfSubclass = GetCandidatePersistentProperties(entityClass, flattenHierarchyBindingFlag);
				IEnumerable<MemberInfo> propertiesOfBaseClass = GetCandidatePersistentProperties(entitySuperclass, flattenHierarchyBindingFlag);
				return propertiesOfSubclass.Except(propertiesOfBaseClass, new PropertyNameEqualityComparer()).Concat(GetUserDeclaredFields(entityClass).Cast<MemberInfo>());
			}
			else
			{
				return GetCandidatePersistentProperties(entityClass, SubClassPropertiesBindingFlags).Concat(GetUserDeclaredFields(entityClass).Cast<MemberInfo>());
			}
		}

		protected IEnumerable<FieldInfo> GetUserDeclaredFields(System.Type type)
		{
			// can't find another way to exclude fields generated by the compiler (for both auto-properties and anonymous-types)
			return type.GetFields(ClassFieldsBindingFlags).Where(x=> !x.Name.StartsWith("<"));
		}

		public IEnumerable<MemberInfo> GetComponentMembers(System.Type componentClass)
		{
			return GetCandidatePersistentProperties(componentClass, ComponentPropertiesBindingFlags).Concat(GetUserDeclaredFields(componentClass).Cast<MemberInfo>());
		}

		#endregion

		private IEnumerable<MemberInfo> GetFieldsOfHierarchy(System.Type type)
		{
			System.Type analizing = type;
			while (analizing != null && analizing != typeof (object))
			{
				foreach (FieldInfo fieldInfo in GetUserDeclaredFields(analizing))
				{
					yield return fieldInfo;
				}
				analizing = analizing.BaseType;
			}
		}

		private IEnumerable<MemberInfo> GetCandidatePersistentProperties(System.Type type, BindingFlags propertiesBindingFlags)
		{
			return type.IsInterface ? type.GetInterfaceProperties() : type.GetProperties(propertiesBindingFlags);
		}

		#region Nested type: PropertyNameEqualityComparer

		private class PropertyNameEqualityComparer : IEqualityComparer<MemberInfo>
		{
			#region Implementation of IEqualityComparer<MemberInfo>

			public bool Equals(MemberInfo x, MemberInfo y)
			{
				return x.Name == y.Name;
			}

			public int GetHashCode(MemberInfo obj)
			{
				return obj.Name.GetHashCode();
			}

			#endregion
		}

		#endregion
	}
}