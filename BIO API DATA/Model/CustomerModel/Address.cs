using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIO_API_DATA.Model.CustomerModel
{
	public class Address
	{
		public string streetCode { get; set; }
		public string streetName { get; set; }
		public string buildingNumber { get; set; }
		public string buildingFloor { get; set; }
		public string roomIdentification { get; set; }
		public string citySubDivisionName { get; set; }
		public string postalCode { get; set; }
		public string city { get; set; }
		public string municipalityCode { get; set; }
		public string countryCode { get; set; }
	}
}
