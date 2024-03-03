using BIO_API_DATA.Model.GasMeterpointToCustomerModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIO_API_DATA.Model.CustomerModel
{
	public class Customer
	{
		public string id { get; set; }
		public string countryCode { get; set; }
		public Identifiers identifiers { get; set; }
		public ContactInformation contactInformation { get; set; }
		public Parent parent { get; set; }
	}
}
