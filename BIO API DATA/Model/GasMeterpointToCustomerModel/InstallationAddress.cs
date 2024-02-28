using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIO_API_DATA.Model.GasMeterpointToCustomerModel
{
	public class InstallationAddress
	{
		public string StreetCode { get; set; }
		public string StreetName { get; set; }
		public string BuildingNumber { get; set; }
		public string BuildingFloor { get; set; }
		public string RoomIdentification { get; set; }
		public string CitySubDivisionName { get; set; }
		public string PostalCode { get; set; }
		public string City { get; set; }
		public string MunicipalityCode { get; set; }
		public string CountryCode { get; set; }
	}
}
