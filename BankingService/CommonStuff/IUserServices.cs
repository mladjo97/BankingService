using System.ServiceModel;

namespace CommonStuff
{
    [ServiceContract]
    public interface IUserServices
    {
        [OperationContract]
        bool OpenAccount(string firstName, string lastName);
    }
}
