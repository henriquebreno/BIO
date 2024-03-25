using BIO_API_DATA.Model;
using BIO_API_DATA.Model.GasMeterpointToCustomerModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BIO_API_DATA.API_Client
{

	public class GasMeteringPointCustomerClient : IGasMeteringPointCustomerClient
	{
		private readonly ILogger _logger;
		private readonly string _baseUrl;
		private readonly IRestClient _restClient;
		public readonly IGasMeteringPointCustomerClientList _customerGas;
		public GasMeteringPointCustomerClient(IConfiguration configuration, ILogger<GasMeteringPointCustomerClient> logger, IRestClient iRestClient, IGasMeteringPointCustomerClientList customersClient)
		{
			_baseUrl = configuration.GetValue<string>("ApiSettings:GasMeteringPointCustomerClient");
			_logger = logger;
			_restClient = iRestClient;
			_customerGas = customersClient;
		}

		public async Task<List<GasMeteringCustomerObjectModel>> GetGasCustomer(List<GasMeterPointCustomerModel> customerGasRelations)
		{
			List<GasMeteringCustomerObjectModel> gasMeteringCustomerObjectModelList = new List<GasMeteringCustomerObjectModel>();
			string url = _baseUrl;
            			
            foreach (var customer in customerGasRelations)
			{
				foreach (var gas in customer.GasMeteringPoints)
				{
					url += $"/api/v1/topLevelCustomers/{customer.CustomerId}/gasMeteringPoints/{gas.Id}";
					var request = new RestRequest(url);
					var response = await _restClient.GetAsync(request);

					if (!response.IsSuccessful)
					{
						throw new Exception($"Error getting gasmetringpointsCustumerRelation: {response.StatusDescription}");
					}

					var content = response.Content;

					var gasMeteringCustomerObjectModel = JsonConvert.DeserializeObject<GasMeteringCustomerObjectModel>(content);

					gasMeteringCustomerObjectModelList.Add(gasMeteringCustomerObjectModel);
				}

			}
			return gasMeteringCustomerObjectModelList;
		}

	}

}
