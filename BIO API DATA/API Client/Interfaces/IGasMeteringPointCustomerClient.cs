using BIO_API_DATA.Model.GasMeterpointToCustomerModel;

namespace BIO_API_DATA.API_Client
{
	public interface IGasMeteringPointCustomerClient
	{
		Task<List<GasMeteringCustomerObjectModel>> GetGasCustomer();
	}
}