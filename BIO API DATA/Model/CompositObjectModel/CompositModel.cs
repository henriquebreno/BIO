using BIO_API_DATA.Model.GasMeterpointToCustomerModel;
using BIO_API_DATA.Model.TimeSeriesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIO_API_DATA.Model.CompositObjectModel
{
	public class CompositModel
	{
        public GasMeteringCustomerObjectModel GasMeteringCustomerObjectModel { get; set; }
        public TimeSeries TimeSeries { get; set; }
        public CustomerModel.Customer Customer { get; set; }
    }
}
