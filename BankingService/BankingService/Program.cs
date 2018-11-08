using CommonStuff.ClientContract;
using System;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Description;

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

            Console.WriteLine("Press <enter> to stop service...");

            Console.ReadLine();
            clientServiceHost.Close();
            adminServiceHost.Close();
        }

        static void StartClientServices()
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/BankingServices";

            clientServiceHost = new ServiceHost(typeof(BankingServices));
            clientServiceHost.AddServiceEndpoint(typeof(IUserServices), binding, address);

            clientServiceHost.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            clientServiceHost.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            clientServiceHost.Open();

            Console.WriteLine("BankingServices service is started.");
        }

        static void StartAdminServices()
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9998/AdminServices";

            adminServiceHost = new ServiceHost(typeof(AdminServices));
            adminServiceHost.AddServiceEndpoint(typeof(IAdminServices), binding, address);

            adminServiceHost.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            adminServiceHost.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            adminServiceHost.Open();

            Console.WriteLine("AdminServices service is started.");
        }

    }
}
