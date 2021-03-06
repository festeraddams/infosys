using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH0000
{
	[TestFixture]
	public class Fixture : BugTestCase
	{
		protected override void OnSetUp()
		{
			using (ISession session = OpenSession())
			using (ITransaction transaction = session.BeginTransaction())
			{
				var e1 = new Entity {Name = "Bob"};
				session.Save(e1);

				var e2 = new Entity {Name = "Sally"};
				session.Save(e2);

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
		public void YourTestName()
		{
			using (ISession session = OpenSession())
			using (session.BeginTransaction())
			{
				var result = from e in session.Query<Entity>()
							 where e.Name == "Bob"
							 select e;

				Assert.AreEqual(1, result.ToList().Count);
			}
		}
	}
}