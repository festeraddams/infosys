using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Test.NHSpecificTest.NH1850
{
	public class Customer
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
	}
}
