using BIO_API_DATA.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIO_API_DATA.API_Client
{
	public partial class TopLevelCustomersClientList : ITopLevelCustomersClientList
	{
		private readonly IConfiguration _configuration;
		private readonly ILogger _logger;
		private readonly IRestClientFactory _restClientFactory;
		private readonly string _baseUrl;
		private readonly IRestClient _restClient;
		public  readonly static List<string> _allCustomerIds = new List<string>();

		public TopLevelCustomersClientList(IConfiguration configuration, ILogger logger, IRestClientFactory restClientFactory, IRestClient iRestClient)
		{
			_baseUrl = configuration.GetValue<string>("ApiSettings:TopLevelCustomersClient");
			_logger = logger;
			_restClientFactory = restClientFactory;
			_restClient = iRestClient;
		}

		public async Task<List<string>> GetAllCustomers()
		{
			string url = _baseUrl + "/api/v1/topLevelCustomers";
			List<string> allCustomerIds = new List<string>();

			try
			{
				_logger.Information("Starting at URL: {Url}", url);

				while (!string.IsNullOrEmpty(url))
				{
					var request = new RestRequest(url);

					var response = await _restClient.GetAsync(request);

					if (!response.IsSuccessful)
					{
						throw new Exception($"Error getting customers: {response.StatusDescription}");
					}

					_logger.Information("Response status: {StatusDescription}", response.StatusDescription);

					var content = response.Content;

					_logger.Debug("Raw JSON response: {Content}", content);

					var responseData = JsonConvert.DeserializeObject<CustomerResponse>(content);

					_logger.Information("Deserialized response: {ResponseData}", responseData);

					if (responseData?.TopLevelCustomerIds != null)
					{
						_logger.Information("Adding {Count} customer IDs:", responseData.TopLevelCustomerIds.Count);
						allCustomerIds.AddRange(responseData.TopLevelCustomerIds);
					}
					else
					{
						_logger.Warning("Missing 'topLevelCustomerIds' property in response");
					}

					url = responseData?.Next;
				}

				return allCustomerIds;

            }
			catch (Exception ex)
			{
				_logger.Error(ex, "Error getting customers: {Message}", ex.Message);
				return default ;
            }
		}

	}
}
