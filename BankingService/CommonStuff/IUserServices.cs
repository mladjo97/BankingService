using System.ServiceModel;

namespace CommonStuff
{
    [ServiceContract]
    public interface IUserServices : IAccountServices, ICreditServices, ITransactionServices
    {
    }
}
