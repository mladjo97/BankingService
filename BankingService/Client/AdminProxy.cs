using CommonStuff.ClientContract;
using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

namespace Client
{
    public class AdminProxy : ChannelFactory<IAdminServices>, IAdminServices
    {
        IAdminServices factory;

        public AdminProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
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
