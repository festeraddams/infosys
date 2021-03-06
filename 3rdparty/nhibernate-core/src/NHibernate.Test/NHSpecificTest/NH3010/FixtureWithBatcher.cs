using NHibernate.Cfg;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH3010
{
	public class FixtureWithBatcher : BugTestCase
	{
		protected override void Configure(Configuration configuration)
		{
			configuration.DataBaseIntegration(x =>
			{
				x.BatchSize = 10;
			});
		}

		protected override void OnSetUp()
		{
			using (ISession session = OpenSession())
			using (ITransaction tx = session.BeginTransaction())
			{
				var parent = new Parent();
				var childOne = new Child();
				parent.Childs.Add(childOne);
				session.Save(parent);

				tx.Commit();
			}
		}

		protected override void OnTearDown()
		{
			using (ISession session = OpenSession())
			using (ITransaction tx = session.BeginTransaction())
			{
				session.Delete("from Child");
				session.Delete("from Parent");
				tx.Commit();
			}
		}

		// Test case from NH-2527
		[Test]
		public void DisposedCommandShouldNotBeReusedAfterRemoveAtAndInsert()
		{
			using (ISession session = OpenSession())
			using (ITransaction tx = session.BeginTransaction())
			{
				var parent = session.CreateCriteria<Parent>().UniqueResult<Parent>();

				Child childOne = parent.Childs[0];

				var childTwo = new Child();
				parent.Childs.Add(childTwo);

				Child childToMove = parent.Childs[1];
				parent.Childs.RemoveAt(1);
				parent.Childs.Insert(0, childToMove);

				Assert.DoesNotThrow(tx.Commit);

				Assert.AreEqual(childTwo.Id, parent.Childs[0].Id);
				Assert.AreEqual(childOne.Id, parent.Childs[1].Id);
			}
		}

		// Test case from NH-1477
		[Test]
		public void DisposedCommandShouldNotBeReusedAfterClearAndAdd()
		{
			using (ISession session = OpenSession())
			using (ITransaction tx = session.BeginTransaction())
			{
				var parent = session.CreateCriteria<Parent>().UniqueResult<Parent>();

				parent.Childs.Clear();

				var childOne = new Child();
				parent.Childs.Add(childOne);

				var childTwo = new Child();
				parent.Childs.Add(childTwo);

				Assert.DoesNotThrow(tx.Commit);

				Assert.AreEqual(childOne.Id, parent.Childs[0].Id);
				Assert.AreEqual(childTwo.Id, parent.Childs[1].Id);
			}
		}
	}
}