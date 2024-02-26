using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BIO_API_DATA.API_Client.TopLevelCustomersClient;

namespace BIO_API_DATA.API_Client
{
	public class MyRestClientFactory : IRestClientFactory
	{
		private readonly string _baseUrl;

		public MyRestClientFactory(string baseUrl)
		{
			_baseUrl = baseUrl;
		}

		public RestClient CreateClient(string url)
		{
			return new RestClient(_baseUrl + url);
		}
	}
}
