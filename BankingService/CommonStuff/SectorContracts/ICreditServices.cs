using System.ServiceModel;

namespace CommonStuff.SectorContracts
{
    [ServiceContract]
    public interface ICreditServices : IStatusFree
    {
        [OperationContract]
        bool TakeLoan(string username,double amount);
    }
}
