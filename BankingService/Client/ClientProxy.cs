using CommonStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientProxy : ChannelFactory<IUserServices>, IUserServices, IDisposable
    {
        IUserServices factory;

        public ClientProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public void TestCall(int num)
        {
            try
            {
                factory.TestCall(num);
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error in ClientProxy.TestCall(): {e.Message}");
            }
        }

    }
}
