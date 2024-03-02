using BIO_API_DATA.Model;

namespace BIO_API_DATA.API_Client
{
	public interface IGasMeteringPointCustomerClientList
	{
		Task<List<GasMeterPointCustomerModel>> GetGasmeteringPointCustomerassociation();
	}
}