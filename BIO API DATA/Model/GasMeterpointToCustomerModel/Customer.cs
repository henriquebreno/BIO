using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIO_API_DATA.Model.GasMeterpointToCustomerModel
{
	public class Customer
	{
		public string Name { get; set; }
		public string Id { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
	}
}
