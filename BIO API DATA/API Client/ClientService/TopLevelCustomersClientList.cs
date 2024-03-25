using BIO_API_DATA.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
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
		private string _baseUrl => _configuration.GetValue<string>("ApiSettings:TopLevelCustomerClient");
		private readonly IRestClient _restClient;

		public TopLevelCustomersClientList(IConfiguration configuration, ILogger<TopLevelCustomersClientList> logger, IRestClient iRestClient)
		{
			_configuration = configuration;
			_logger = logger;
			_restClient = iRestClient;
		}
	 
		public async Task<List<string>> GetAllCustomers()
		{
			string url = _baseUrl + "/api/v1/topLevelCustomers";
			List<string> allCustomerIds = new List<string>();

			try
			{
				_logger.LogInformation("Starting at URL: {Url}", url);

				while (!string.IsNullOrEmpty(url))
				{
					var request = new RestRequest(url);

					var response = await _restClient.GetAsync(request);

					if (!response.IsSuccessful)
					{
						throw new Exception($"Error getting customers: {response.StatusDescription}");
					}

					_logger.LogInformation("Response status: {StatusDescription}", response.StatusDescription);

					var content = response.Content;

                    _logger.LogDebug("Raw JSON response: {Content}", content);

					var responseData = JsonConvert.DeserializeObject<CustomerResponse>(content);

					_logger.LogInformation("Deserialized response: {ResponseData}", responseData);

					if (responseData?.TopLevelCustomerIds != null)
					{
						_logger.LogInformation("Adding {Count} customer IDs:", responseData.TopLevelCustomerIds.Count);
						allCustomerIds.AddRange(responseData.TopLevelCustomerIds);
					}
					else
					{
						_logger.LogWarning("Missing 'topLevelCustomerIds' property in response");
					}

					url = responseData?.Next;
				}


				return allCustomerIds;


            }
			catch (Exception ex)
			{

                _logger.LogError(ex, "Error getting customers: {Message}", ex.Message);
				throw ex;
			}
		}
	}
}
