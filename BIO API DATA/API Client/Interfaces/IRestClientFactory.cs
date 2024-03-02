using RestSharp;

namespace BIO_API_DATA.API_Client
{
	public partial class TopLevelCustomersClientList
	{
		public interface IRestClientFactory
		{
			RestClient CreateClient(string baseUrl);
		}
	}
}
