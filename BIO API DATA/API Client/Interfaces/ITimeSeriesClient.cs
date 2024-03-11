using BIO_API_DATA.Model.CompositObjectModel;
using BIO_API_DATA.Model.GasMeterpointToCustomerModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIO_API_DATA.API_Client.Interfaces
{
    public interface ITimeSeriesClient
    {
        Task<List<CompositModel>> GetTimeSeries(List<string> customerIds, List<GasMeteringCustomerObjectModel> customerGasRelations);
    }
}
