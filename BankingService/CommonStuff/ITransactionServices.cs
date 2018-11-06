using System.ServiceModel;

namespace CommonStuff
{
    [ServiceContract]
    public interface ITransactionServices
    {
        [OperationContract]
        bool DoTransaction(string username, TransactionType type, double amount);
    }
}
