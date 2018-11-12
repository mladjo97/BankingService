using CommonStuff.ClientContract;
using CertificationManager;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;

namespace BankingService
{
    class Program
    {
        private static string bankingSectorsPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory() , @"..\..\..\BankingSectors\bin\Debug\BankingSectors.exe"));

        private static ServiceHost clientServiceHost;
        private static ServiceHost adminServiceHost;

        static void Main(string[] args)
        {
            StartClientServices();
            StartAdminServices();

            // pokreni BankingSector
            Process.Start(bankingSectorsPath);

            Console.WriteLine("Press <enter> to stop service...\n");

            Console.ReadLine();
            clientServiceHost.Close();
            adminServiceHost.Close();
        }

        static void StartClientServices()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            string address = "net.tcp://localhost:9999/BankingServices";

            clientServiceHost = new ServiceHost(typeof(BankingServices));
            clientServiceHost.AddServiceEndpoint(typeof(IUserServices), binding, address);

            clientServiceHost.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            clientServiceHost.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            // sertifikati
            clientServiceHost.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            clientServiceHost.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            clientServiceHost.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, "bankingservice");

            try
            {
                clientServiceHost.Open();
                Console.WriteLine("BankingServices service is started.");
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR in BankingServices] {0}", e.Message);
            }
        }

        static void StartAdminServices()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            string address = "net.tcp://localhost:9998/AdminServices";

            adminServiceHost = new ServiceHost(typeof(AdminServices));
            adminServiceHost.AddServiceEndpoint(typeof(IAdminServices), binding, address);

            adminServiceHost.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            adminServiceHost.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            // sertifikati
            adminServiceHost.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            adminServiceHost.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            adminServiceHost.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, "bankingservice");

            try
            {
                adminServiceHost.Open();
                Console.WriteLine("AdminServices service is started.");
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR in AdminServices] {0}", e.Message);
            }

        }

    }
}
