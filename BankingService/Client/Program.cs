using System;
using System.ServiceModel;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {

            NetTcpBinding binding = new NetTcpBinding();
            string clientAddress = "net.tcp://localhost:9999/BankingServices";
            string adminAddress = "net.tcp://localhost:9998/AdminServices";

            // ucitamo username
            string username = Console.ReadLine();

            using (ClientProxy proxy = new ClientProxy(binding, clientAddress))
            {
                // otvori racun                
                if (proxy.OpenAccount(username))
                    Console.WriteLine("Success! Account opened.");
                else
                    Console.WriteLine("Fail! Account was not opened.");
            }

            // test za admina
            using (AdminProxy adminProxy = new AdminProxy(binding, adminAddress))
            {
                adminProxy.CreateDB();
                adminProxy.CheckRequests();
            }

            Console.ReadLine();
        }
    }
}

// uzmi kredit
//if(proxy.TakeLoan("Mladen97",5000))
//    Console.WriteLine("Success! Loan taken.");
//else
//    Console.WriteLine("Fail! Loan was not approved.");

//// uplati novac
//if(proxy.DoTransaction("Mladen97",TransactionType.Deposit, 3000))
//    Console.WriteLine("Success! You deposited in your account.");
//else
//    Console.WriteLine("Fail! You were unable to deposit in your account.");