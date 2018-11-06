using System.ServiceModel;

namespace CommonStuff
{
    [ServiceContract]
    public interface ICreditServices
    {
        [OperationContract]
        bool TakeLoan(double amount);
    }
}
