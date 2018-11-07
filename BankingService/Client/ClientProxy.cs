using CommonStuff;
using CommonStuff.ClientContract;
using System;
using System.ServiceModel;

namespace Client
{
    public class ClientProxy : ChannelFactory<IUserServices>, IUserServices
    {
        IUserServices factory;

        public ClientProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public bool OpenAccount(string username)
        {
            bool result = false;

            try
            {
                result = factory.OpenAccount(username);
                Console.WriteLine($"OpenAccount() >> {result}");
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in ClientProxy.OpenAccount(): {e.Message}");
            }

            return result;
        }

        public bool TakeLoan(string username, double amount)
        {
            bool result = false;

            try
            {
                result = factory.TakeLoan(username,amount);
                Console.WriteLine($"TakeLoan() >> {result}");
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in ClientProxy.TakeLoan(): {e.Message}");
            }

            return result;
        }

        public bool DoTransaction(string username, TransactionType type, double amount)
        {
            bool result = false;

            try
            {
                result = factory.DoTransaction(username, type, amount);
                Console.WriteLine($"DoTransaction() >> {result}");
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in ClientProxy.DoTransaction(): {e.Message}");
            }

            return result;
        }

    }
}
