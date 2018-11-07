using System.ServiceModel;

namespace CommonStuff.ClientContract
{
    [ServiceContract]
    public interface IAdminServices
    {
        [OperationContract]
        void CreateDB();

        [OperationContract]
        void CheckRequests();
    }
}
