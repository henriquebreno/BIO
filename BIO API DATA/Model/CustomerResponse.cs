using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIO_API_DATA.Model
{
	public class CustomerResponse
	{
		public List<string> TopLevelCustomerIds { get; set; }
		public string Next { get; set; }
		public string Prev { get; set; }
	}
}
