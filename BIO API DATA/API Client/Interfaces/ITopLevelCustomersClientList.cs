namespace BIO_API_DATA.API_Client
{
	public interface ITopLevelCustomersClientList
	{
        Task<List<string>> GetAllCustomers();

    }
}