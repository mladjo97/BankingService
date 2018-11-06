using CommonStuff;
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

        public bool OpenAccount(string firstName, string lastName)
        {
            bool result = false;

            try
            {
                result = factory.OpenAccount(firstName, lastName);
                Console.WriteLine($"OpenAccount() >> {result}");
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in ClientProxy.OpenAccount(): {e.Message}");
            }

            return result;
        }

        public bool TakeLoan(double amount)
        {
            bool result = false;

            try
            {
                result = factory.TakeLoan(amount);
                Console.WriteLine($"TakeLoan() >> {result}");
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in ClientProxy.TakeLoan(): {e.Message}");
            }

            return result;
        }

        public bool DoTransaction(TransactionType type, double amount)
        {
            bool result = false;

            try
            {
                result = factory.DoTransaction(type, amount);
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
