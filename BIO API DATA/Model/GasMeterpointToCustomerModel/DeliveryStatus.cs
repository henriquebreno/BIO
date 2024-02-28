using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIO_API_DATA.Model.GasMeterpointToCustomerModel
{
	public class DeliveryStatus
	{
		public bool InDelivery { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
	}
}
