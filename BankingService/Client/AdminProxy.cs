using CommonStuff.ClientContract;
using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

namespace Client
{
    public class AdminProxy : ChannelFactory<IAdminServices>, IAdminServices
    {
        IAdminServices factory;

        public AdminProxy(NetTcpBinding binding, EndpointAddress address, X509Certificate2 cert) : base(binding, address)
        {
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            this.Credentials.ClientCertificate.Certificate = cert;

            factory = this.CreateChannel();
        }

        public bool CheckRequests()
        {
            bool result = false;

            try
            {
                result = factory.CheckRequests();       // proveri jednom, ovde neki while ili nesto, samo da ponavlja
                return result;
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine($"Error in AdminProxy.CheckRequest(): {e.Message}");
            }

            return result;
        }

        public bool CreateDB()
        {
            bool result = false;

            try
            {
                result = factory.CreateDB();
                return result;
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine($"Error in AdminProxy.CreateDB(): {e.Message}");
            }

            return result;
        }
    }
}
