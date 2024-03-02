using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BIO_API_DATA.API_Client.TopLevelCustomersClientList;

namespace BIO_API_DATA.API_Client
{
	public class MyRestClientFactory : IRestClientFactory
	{
		private readonly IConfiguration _configuration;
		private readonly string _baseUrl;

		public MyRestClientFactory(IConfiguration configuration)
		{
			_baseUrl = configuration.GetValue<string>("ApiSettings:MyRestClientFactory");
		}

		public RestClient CreateClient(string url)
		{
			return new RestClient(_baseUrl + url);
		}
	}
}
