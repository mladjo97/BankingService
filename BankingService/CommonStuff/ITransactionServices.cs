using System.ServiceModel;

namespace CommonStuff
{
    [ServiceContract]
    public interface ITransactionServices
    {
        [OperationContract]
        bool DoTransaction(TransactionType type, double amount);
    }
}
