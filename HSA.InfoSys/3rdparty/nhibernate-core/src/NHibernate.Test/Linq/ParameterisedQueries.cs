using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Linq;
using NHibernate.DomainModel.Northwind.Entities;
using NUnit.Framework;

namespace NHibernate.Test.Linq
{
	[TestFixture]
	public class ParameterisedQueries : ReadonlyTestCase
	{
		[Test]
		public void Identical_Expressions_Return_The_Same_Key()
		{
			using (var s = OpenSession())
			{
				var db = new Northwind(s);

				Expression<Func<IEnumerable<Customer>>> london1 =
					() => from c in db.Customers where c.Address.City == "London" select c;
				Expression<Func<IEnumerable<Customer>>> london2 =
					() => from c in db.Customers where c.Address.City == "London" select c;

				var nhLondon1 = new NhLinqExpression(london1.Body, sessions);
				var nhLondon2 = new NhLinqExpression(london2.Body, sessions);

				Assert.AreEqual(nhLondon1.Key, nhLondon2.Key);
			}
		}

		[Test]
		public void Expressions_Differing_Only_By_Constants_Return_The_Same_Key()
		{
			using (var s = OpenSession())
			{
				var db = new Northwind(s);

				Expression<Func<IEnumerable<Customer>>> london =
					() => from c in db.Customers where c.Address.City == "London" select c;

				Expression<Func<IEnumerable<Customer>>> newYork =
					() => from c in db.Customers where c.Address.City == "New York" select c;

				var nhLondon = new NhLinqExpression(london.Body, sessions);
				var nhNewYork = new NhLinqExpression(newYork.Body, sessions);

				Assert.AreEqual(nhLondon.Key, nhNewYork.Key);
				Assert.AreEqual(1, nhLondon.ParameterValuesByName.Count);
				Assert.AreEqual(1, nhNewYork.ParameterValuesByName.Count);
				Assert.AreEqual("London", nhLondon.ParameterValuesByName.First().Value.Item1);
				Assert.AreEqual("New York", nhNewYork.ParameterValuesByName.First().Value.Item1);
			}
		}

		[Test]
		public void Different_Where_Clauses_Return_Different_Keys()
		{
			using (var s = OpenSession())
			{
				var db = new Northwind(s);

				Expression<Func<IEnumerable<Customer>>> london =
					() => from c in db.Customers where c.Address.City == "London" select c;
				Expression<Func<IEnumerable<Customer>>> company =
					() => from c in db.Customers where c.CompanyName == "Acme" select c;

				var nhLondon = new NhLinqExpression(london.Body, sessions);
				var nhNewYork = new NhLinqExpression(company.Body, sessions);

				Assert.AreNotEqual(nhLondon.Key, nhNewYork.Key);
			}
		}

		[Test]
		public void Different_Select_Properties_Return_Different_Keys()
		{
			using (var s = OpenSession())
			{
				var db = new Northwind(s);

				Expression<Func<IEnumerable<string>>> customerId =
					() => from c in db.Customers select c.CustomerId;
				Expression<Func<IEnumerable<string>>> title =
					() => from c in db.Customers select c.ContactTitle;

				var nhLondon = new NhLinqExpression(customerId.Body, sessions);
				var nhNewYork = new NhLinqExpression(title.Body, sessions);

				Assert.AreNotEqual(nhLondon.Key, nhNewYork.Key);
			}
		}

		[Test]
		public void Different_Select_Types_Return_Different_Keys()
		{
			using (var s = OpenSession())
			{
				var db = new Northwind(s);

				Expression<Func<IEnumerable>> newCustomerId =
					() => from c in db.Customers select new { c.CustomerId };
				Expression<Func<IEnumerable>> customerId =
					() => from c in db.Customers select c.CustomerId;

				var nhLondon = new NhLinqExpression(newCustomerId.Body, sessions);
				var nhNewYork = new NhLinqExpression(customerId.Body, sessions);

				Assert.AreNotEqual(nhLondon.Key, nhNewYork.Key);
			}
		}

		[Test]
		public void Different_Select_Member_Initialisation_Returns_Different_Keys()
		{
			using (var s = OpenSession())
			{
				var db = new Northwind(s);

				Expression<Func<IEnumerable>> newCustomerId =
					() => from c in db.Customers select new { Id = c.CustomerId, Title = c.ContactTitle };
				Expression<Func<IEnumerable>> customerId =
					() => from c in db.Customers select new { Title = c.ContactTitle, Id = c.CustomerId };

				var nhLondon = new NhLinqExpression(newCustomerId.Body, sessions);
				var nhNewYork = new NhLinqExpression(customerId.Body, sessions);

				Assert.AreNotEqual(nhLondon.Key, nhNewYork.Key);
			}
		}

		[Test]
		public void Different_Conditionals_Return_Different_Keys()
		{
			using (var s = OpenSession())
			{
				var db = new Northwind(s);

				Expression<Func<IEnumerable>> newCustomerId =
					() => from c in db.Customers select new { Desc = c.CustomerId == "1" ? "First" : "Not First" };
				Expression<Func<IEnumerable>> customerId =
					() => from c in db.Customers select new { Desc = c.CustomerId != "1" ? "First" : "Not First" };

				var nhLondon = new NhLinqExpression(newCustomerId.Body, sessions);
				var nhNewYork = new NhLinqExpression(customerId.Body, sessions);

				Assert.AreNotEqual(nhLondon.Key, nhNewYork.Key);
			}
		}

		[Test]
		public void Different_Unary_Operation_Returns_Different_Keys()
		{
			using (var s = OpenSession())
			{
				var db = new Northwind(s);

				Expression<Func<IEnumerable>> newCustomerId =
					() => from c in db.Customers where c.CustomerId == "1" select c;
				Expression<Func<IEnumerable>> customerId =
					() => from c in db.Customers where !(c.CustomerId == "1") select c;

				var nhLondon = new NhLinqExpression(newCustomerId.Body, sessions);
				var nhNewYork = new NhLinqExpression(customerId.Body, sessions);

				Assert.AreNotEqual(nhLondon.Key, nhNewYork.Key);
			}
		}

		[Test]
		public void Different_OfType_Returns_Different_Keys()
		{
			using (var session = OpenSession())
			{
				Expression<Func<IEnumerable>> ofType1 = () => (from a in session.Query<Animal>().OfType<Cat>() where a.Pregnant select a.Id);
				Expression<Func<IEnumerable>> ofType2 = () => (from a in session.Query<Animal>().OfType<Dog>() where a.Pregnant select a.Id);

				var nhOfType1 = new NhLinqExpression(ofType1.Body, sessions);
				var nhOfType2 = new NhLinqExpression(ofType2.Body, sessions);

				Assert.AreNotEqual(nhOfType1.Key, nhOfType2.Key);
			}
		}

		[Test]
		public void Different_Null_Returns_Different_Keys()
		{
			using (var session = OpenSession())
			{
				string nullVariable = null;
				string notNullVariable = "Hello";

				Expression<Func<IEnumerable>> null1 = () => (from a in session.Query<Animal>() where a.Description == null select a);
				Expression<Func<IEnumerable>> null2 = () => (from a in session.Query<Animal>() where a.Description == nullVariable select a);
				Expression<Func<IEnumerable>> notNull = () => (from a in session.Query<Animal>() where a.Description == notNullVariable select a);

				var nhNull1 = new NhLinqExpression(null1.Body, sessions);
				var nhNull2 = new NhLinqExpression(null2.Body, sessions);
				var nhNotNull = new NhLinqExpression(notNull.Body, sessions);

				Assert.AreNotEqual(nhNull1.Key, nhNotNull.Key);
				Assert.AreNotEqual(nhNull2.Key, nhNotNull.Key);
			}
		}

		// TODO - different parameter names

		protected override IList Mappings
		{
			get { return new string[0]; }
		}
	}
}