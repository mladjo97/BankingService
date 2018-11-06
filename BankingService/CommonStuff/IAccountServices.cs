using System.ServiceModel;

namespace CommonStuff
{
    [ServiceContract]
    public interface IAccountServices
    {
        [OperationContract]
        bool OpenAccount(string firstName, string lastName);
    }
}
