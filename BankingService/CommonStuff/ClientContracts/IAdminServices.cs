using System.ServiceModel;

namespace CommonStuff.ClientContract
{
    [ServiceContract]
    public interface IAdminServices
    {
        [OperationContract]
        bool CreateDB();

        [OperationContract]
        bool CheckRequests();
    }
}
