using System.ServiceModel;

namespace CommonStuff.ClientContract
{
    [ServiceContract]
    public interface IUserServices
    {
        [OperationContract]
        bool OpenAccount(string username);

        [OperationContract]
        bool TakeLoan(string username, double amount);

        [OperationContract]
        bool DoTransaction(string username, TransactionType type, double amount);

        [OperationContract]
        AccountInfo GetAccountInfo(string username);
    }
}
