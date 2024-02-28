using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIO_API_DATA.Model.GasMeterpointToCustomerModel
{
	public class GasMeteringCustomerObjectModel
	{
		public string Id { get; set; }
		public string CountryCode { get; set; }
		public Customer Customer { get; set; }
		public InstallationAddress InstallationAddress { get; set; }
		public Identifiers Identifiers { get; set; }
		public DeliveryStatus DeliveryStatus { get; set; }
		public GridAreaDto GridAreaDto { get; set; }
		public MarketBalanceArea MarketBalanceArea { get; set; }
		public CalorificValueArea CalorificValueArea { get; set; }
	}
}
