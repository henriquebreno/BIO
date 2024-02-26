
namespace BIO_API_DATA.API_Client
{
	public interface ITopLevelCustomersClient
	{
		Task<List<string>> GetAllCustomers();
	}
}