using CommonStuff;
using System;
using System.ServiceModel;

namespace Client
{
    public class ClientProxy : ChannelFactory<IUserServices>, IUserServices, IDisposable
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
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in ClientProxy.TestCall(): {e.Message}");
            }

            return result;
        }

    }
}
