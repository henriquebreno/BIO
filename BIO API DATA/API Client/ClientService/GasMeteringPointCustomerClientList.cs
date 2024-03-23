using BIO_API_DATA.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BIO_API_DATA.API_Client.TopLevelCustomersClientList;

namespace BIO_API_DATA.API_Client
{
	public class GasMeteringPointCustomerClientList : IGasMeteringPointCustomerClientList
	{
		private readonly ILogger _logger;
		private readonly string _baseUrl;
		private readonly IRestClient _restClient;
		public readonly ITopLevelCustomersClientList _customersClient;

		public GasMeteringPointCustomerClientList(IConfiguration configuration, ILogger logger, IRestClient iRestClient, ITopLevelCustomersClientList customersClient)
		{
			_baseUrl = configuration.GetValue<string>("ApiSettings:GasMeteringPointClient");
			_logger = logger;
			_restClient = iRestClient;
			_customersClient = customersClient;
		}

		public async Task<List<GasMeterPointCustomerModel>> GetGasmeteringPointCustomerassociation(List<string> CustomerIdList)
		{
			var GasMeterPointCustomerIDList = new List<GasMeterPointCustomerModel>();

			string url;

			foreach (var id in CustomerIdList)
			{
				url = _baseUrl + $"/api/v1/topLevelCustomers/{id}/gasMeteringPoints?associationFilter=0";
				_logger.Information("Starting at URL: {Url}", url);

				while (!string.IsNullOrEmpty(url))
				{
					var request = new RestRequest(url);
					var response = await _restClient.GetAsync(request);

					if (!response.IsSuccessful)
					{
						throw new Exception($"Error getting gasmetringpoints: {response.StatusDescription}");
					}

					_logger.Information("Response status: {StatusDescription}", response.StatusDescription);

					var content = response.Content;

					_logger.Debug("Raw JSON response: {Content}", content);

					var responseData = JsonConvert.DeserializeObject<GasMeteringPointResponse>(content);

					_logger.Information("Deserialized response: {ResponseData}", responseData);

					if (responseData?.GasMeteringPoints != null)
					{
						_logger.Information("Adding customer ids with gasmeteringpoints");
						var data = new GasMeterPointCustomerModel();
						data.CustomerId = id;
						data.GasMeteringPoints.AddRange(responseData.GasMeteringPoints);
					}
					else
					{
						_logger.Warning("No Gasmeteringpoints found");
					}

					url = responseData?.Next;
				}

			}

			return GasMeterPointCustomerIDList;


		}
	}
}
