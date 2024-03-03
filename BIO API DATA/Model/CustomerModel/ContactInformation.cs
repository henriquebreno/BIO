using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BIO_API_DATA.Model.CustomerModel
{
	public class ContactInformation
	{
		public Address address { get; set; }
		public CustomerName customerName { get; set; }
	}
}
