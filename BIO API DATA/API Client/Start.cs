using BIO_API_DATA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIO_API_DATA.API_Client
{
	public class Start
	{

		public void Run()
		{
			var context = new BioDataContext();

			var gasmeteringpoint = new GasMeteringPoint
			{
				Id = 1,
				MeteringPointIdentification = 1,
			};

			var gasMeterCustomerRelation = new GasMeterCustomerRelation
			{
				Id = 1,
				GasMeteringPointId = 1,
			};

			var customer = new Customer
			{
				Id = 1,

			};

			



		}

	}
}
