using System.ServiceModel;

namespace CommonStuff
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
