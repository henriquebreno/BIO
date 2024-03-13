using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIO_API_DATA.API_Client.Interfaces
{
    public interface ITimeSeriesClient
    {
        Task<Model.CustomerModel.Customer> GetCustomerInfo(string topLevelCustomerId, string customerId);
        Task GetTimeSeries();
    }
}
