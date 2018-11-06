using CommonStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BankingSectors
{
    class Program
    {
        static void Main(string[] args)
        {
            OpenAccountServices();
            Console.ReadLine();
        }

        static void OpenAccountServices()
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9990/AccountServices";

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.Sign;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;

            ServiceHost host = new ServiceHost(typeof(AccountServices));
            host.AddServiceEndpoint(typeof(IAccountServices), binding, address);

            //host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            //host.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            host.Open();
        }

        static void OpenTransactionServices()
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9991/TransactionServices";

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.Sign;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;

            ServiceHost host = new ServiceHost(typeof(TransactionServices));
            host.AddServiceEndpoint(typeof(ITransactionServices), binding, address);

            //host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            //host.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            host.Open();
        }

       

        static void OpenCreditServices()
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9992/CreditServices";

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.Sign;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;

            ServiceHost host = new ServiceHost(typeof(CreditServices));
            host.AddServiceEndpoint(typeof(ICreditServices), binding, address);

            //host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            //host.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            host.Open();
        }
    }
}
