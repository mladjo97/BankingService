using System.ServiceModel;

namespace CommonStuff
{
    [ServiceContract]
    public interface IUserServices
    {
        [OperationContract]
        void TestCall(int num);
    }
}
