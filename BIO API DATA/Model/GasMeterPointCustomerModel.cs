using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIO_API_DATA.Model
{
	public class GasMeterPointCustomerModel
	{
        public string CustomerId { get; set; }
        public List<GasMeteringPoint> GasMeteringPoints { get; set; }
    }
}
