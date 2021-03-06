using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System;

namespace NHibernate.Test.NHSpecificTest.NH3237
{
	[TestFixture]
	public class ByCodeFixture : TestCaseMappingByCode
	{
		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Entity>(rc =>
			{
				rc.Id(x => x.Id, m => m.Generator(Generators.GuidComb));
				rc.Property(x => x.DateTimeOffsetValue, m => m.Type(typeof(DateTimeOffsetUserType), new DateTimeOffsetUserType(TimeSpan.FromHours(10))));
				rc.Property(x => x.EnumValue, m => m.Type(typeof(EnumUserType), null));
				rc.Property(x => x.IntValue);
				rc.Property(x => x.LongValue);
				rc.Property(x => x.DecimalValue);
				rc.Property(x => x.DoubleValue);
				rc.Property(x => x.FloatValue);
				rc.Property(x => x.DateTimeValue);
				rc.Property(x => x.StringValue);
			});

			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		protected override void OnSetUp()
		{
			using (ISession session = OpenSession())
			using (ITransaction transaction = session.BeginTransaction())
			{
				var e1 = new Entity
				{
					DateTimeOffsetValue = new DateTimeOffset(2012, 08, 06, 11, 0, 0, TimeSpan.FromHours(10)),
					EnumValue = TestEnum.Zero,
					IntValue = 1,
					LongValue = 1L,
					DecimalValue = 1.2m,
					DoubleValue = 1.2d,
					FloatValue = 1.2f,
					DateTimeValue = new DateTime(2012, 08, 06, 11, 0, 0),
					StringValue = "a"
				};
				session.Save(e1);

				var e2 = new Entity
				{
					DateTimeOffsetValue = new DateTimeOffset(2012, 08, 06, 12, 0, 0, TimeSpan.FromHours(10)),
					EnumValue = TestEnum.One,
					IntValue = 2,
					LongValue = 2L,
					DecimalValue = 2.2m,
					DoubleValue = 2.2d,
					FloatValue = 2.2f,
					DateTimeValue = new DateTime(2012, 08, 06, 12, 0, 0),
					StringValue = "b"
				};
				session.Save(e2);

				var e3 = new Entity
				{
					DateTimeOffsetValue = new DateTimeOffset(2012, 08, 06, 13, 0, 0, TimeSpan.FromHours(10)),
					EnumValue = TestEnum.Two,
					IntValue = 3,
					LongValue = 3L,
					DecimalValue = 3.2m,
					DoubleValue = 3.2d,
					FloatValue = 3.2f,
					DateTimeValue = new DateTime(2012, 08, 06, 13, 0, 0),
					StringValue = "c"
				};
				session.Save(e3);

				session.Flush();
				transaction.Commit();
			}
		}

		protected override void OnTearDown()
		{
			using (ISession session = OpenSession())
			using (ITransaction transaction = session.BeginTransaction())
			{
				session.Delete("from System.Object");

				session.Flush();
				transaction.Commit();
			}
		}

		[Test]
		public void Test_That_DateTimeOffset_UserType_Can_Be_Used_For_Max_And_Min_Aggregates()
		{
			using (ISession session = OpenSession())
			using (session.BeginTransaction())
			{
				var min = session.Query<Entity>().Min(e => e.DateTimeOffsetValue);
				Assert.AreEqual(new DateTimeOffset(2012, 08, 06, 11, 0, 0, TimeSpan.FromHours(10)), min);

				var max = session.Query<Entity>().Max(e => e.DateTimeOffsetValue);
				Assert.AreEqual(new DateTimeOffset(2012, 08, 06, 13, 0, 0, TimeSpan.FromHours(10)), max);
			}
		}

		[Test]
		public void Test_That_Enum_Type_Can_Be_Used_For_Max_And_Min_Aggregates()
		{
			using (ISession session = OpenSession())
			using (session.BeginTransaction())
			{
				var min = session.Query<Entity>().Min(e => e.EnumValue);
				Assert.AreEqual(TestEnum.Zero, min);

				var max = session.Query<Entity>().Max(e => e.EnumValue);
				Assert.AreEqual(TestEnum.Two, max);
			}
		}

		[Test]
		public void Test_Max_And_Min_Aggregates_Work_For_Ints()
		{
			using (ISession session = OpenSession())
			using (session.BeginTransaction())
			{
				var min = session.Query<Entity>().Min(e => e.IntValue);
				Assert.AreEqual(1, min);

				var max = session.Query<Entity>().Max(e => e.IntValue);
				Assert.AreEqual(3, max);
			}
		}

		[Test]
		public void Test_Max_And_Min_Aggregates_Work_For_Longs()
		{
			using (ISession session = OpenSession())
			using (session.BeginTransaction())
			{
				var min = session.Query<Entity>().Min(e => e.LongValue);
				Assert.AreEqual(1L, min);

				var max = session.Query<Entity>().Max(e => e.LongValue);
				Assert.AreEqual(3L, max);
			}
		}

		[Test]
		public void Test_Max_And_Min_Aggregates_Work_For_Decimals()
		{
			using (ISession session = OpenSession())
			using (session.BeginTransaction())
			{
				var min = session.Query<Entity>().Min(e => e.DecimalValue);
				Assert.AreEqual(1.2m, min);

				var max = session.Query<Entity>().Max(e => e.DecimalValue);
				Assert.AreEqual(3.2m, max);
			}
		}

		[Test]
		public void Test_Max_And_Min_Aggregates_Work_For_Doubles()
		{
			using (ISession session = OpenSession())
			using (session.BeginTransaction())
			{
				var min = session.Query<Entity>().Min(e => e.DoubleValue);
				Assert.AreEqual(1.2d, min);

				var max = session.Query<Entity>().Max(e => e.DoubleValue);
				Assert.AreEqual(3.2d, max);
			}
		}

		[Test]
		public void Test_Max_And_Min_Aggregates_Work_For_Floats()
		{
			using (ISession session = OpenSession())
			using (session.BeginTransaction())
			{
				var min = session.Query<Entity>().Min(e => e.FloatValue);
				Assert.AreEqual(1.2f, min);

				var max = session.Query<Entity>().Max(e => e.FloatValue);
				Assert.AreEqual(3.2f, max);
			}
		}

		[Test]
		public void Test_Max_And_Min_Aggregates_Work_For_DateTimes()
		{
			using (ISession session = OpenSession())
			using (session.BeginTransaction())
			{
				var min = session.Query<Entity>().Min(e => e.DateTimeValue);
				Assert.AreEqual(new DateTime(2012, 08, 06, 11, 0, 0), min);

				var max = session.Query<Entity>().Max(e => e.DateTimeValue);
				Assert.AreEqual(new DateTime(2012, 08, 06, 13, 0, 0), max);
			}
		}

		[Test]
		public void Test_Max_And_Min_Aggregates_Work_For_Strings()
		{
			using (ISession session = OpenSession())
			using (session.BeginTransaction())
			{
				var min = session.Query<Entity>().Min(e => e.StringValue);
				Assert.AreEqual("a", min);

				var max = session.Query<Entity>().Max(e => e.StringValue);
				Assert.AreEqual("c", max);
			}
		}
	}
}
