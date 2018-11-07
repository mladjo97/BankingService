using System.ServiceModel;

namespace CommonStuff.SectorContracts
{
    [ServiceContract]
    public interface IAccountServices : IStatusFree
    {
        [OperationContract]
        bool OpenAccount(string username);
    }
}
