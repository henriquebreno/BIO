using BIO_API_DATA.Model;

namespace BIO_API_DATA.API_Client
{
	public interface IGasMeteringPointCustomerListClient
	{
		Task<List<GasMeterPointCustomerModel>> GetGasmetringPointCustomerassociation();
	}
}