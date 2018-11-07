using System.ServiceModel;

namespace CommonStuff.SectorContracts
{
    [ServiceContract]
    public interface ITransactionServices : IStatusFree
    {
        [OperationContract]
        bool DoTransaction(string username, TransactionType type, double amount);
    }
}
