using System.ServiceModel;

namespace CommonStuff
{
    [ServiceContract]
    public interface IUserServices
    {
        [OperationContract]
        bool OpenAccount(string firstName, string lastName);

        [OperationContract]
        bool TakeLoan(double amount);

        [OperationContract]
        bool DoTransaction(TransactionType type, double amount);
    }
}
