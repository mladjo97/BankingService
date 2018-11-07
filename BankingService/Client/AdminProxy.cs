using CommonStuff.ClientContract;
using System;
using System.ServiceModel;

namespace Client
{
    public class AdminProxy : ChannelFactory<IAdminServices>, IAdminServices
    {
        IAdminServices factory;

        public AdminProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public void CheckRequests()
        {
            try
            {
                factory.CheckRequests();       // proveri jednom, ovde neki while ili nesto, samo da ponavlja         
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in AdminProxy.CheckRequest(): {e.Message}");
            }
        }

        public void CreateDB()
        {
            try
            {
                factory.CreateDB();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in AdminProxy.CreateDB(): {e.Message}");
            }
        }
    }
}
