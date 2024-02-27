using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BIO_API_DATA.API_Client.TopLevelCustomersClient;

namespace BIO_API_DATA.API_Client
{
	public class GasMeteringPointClient : IGasMeteringPointClient
	{
		private readonly ILogger _logger;
		private readonly string _baseUrl;
		private readonly IRestClient _restClient;
		private readonly ITopLevelCustomersClient _customersClient;

		public GasMeteringPointClient(IConfiguration configuration, ILogger logger, IRestClient iRestClient, ITopLevelCustomersClient customersClient)
		{
			_baseUrl = configuration.GetValue<string>("ApiSettings:GasMeteringPointClient");
			_logger = logger;
			_restClient = iRestClient;
			_customersClient = customersClient;
		}

		public void GetGasmetringPointCustomerassociation()
		{

			var IdList = _customersClient.GetAllCustomers();

			foreach (var id in IdList.Result)
			{
				string url = _baseUrl + $"/api/v1/topLevelCustomers/{id}/gasMeteringPoints?associationFilter=0";



			}




		}

		public void SaveGasCustomerRelation()
		{

		}
	}
}
