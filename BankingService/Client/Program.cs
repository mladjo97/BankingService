using CommonStuff;
using System;
using System.ServiceModel;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // da ne pravimo dve razlicite konzole, mozemo ovde da proverimo da li je User u grupi Admins
            // i da napravimo UI tako da ima odredjene opcije

            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/BankingServices";

            using (ClientProxy proxy = new ClientProxy(binding, address))
            {
                // otvori racun
                if (proxy.OpenAccount("Mladen", "Milosevic"))
                    Console.WriteLine("Success! Account opened.");
                else
                    Console.WriteLine("Fail! Account was not opened.");

                // uzmi kredit
                if(proxy.TakeLoan(5000))
                    Console.WriteLine("Success! Loan taken.");
                else
                    Console.WriteLine("Fail! Loan was not approved.");

                // uplati novac
                if(proxy.DoTransaction(TransactionType.Deposit, 3000))
                    Console.WriteLine("Success! You deposited in your account.");
                else
                    Console.WriteLine("Fail! You were unable to deposit in your account.");

            }

            Console.ReadLine();
        }
    }
}
