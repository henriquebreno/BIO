using RestSharp;

namespace BIO_API_DATA.API_Client
{
	public partial class TopLevelCustomersClient
	{
		public interface IRestClientFactory
		{
			RestClient CreateClient(string baseUrl);
		}
	}
}
