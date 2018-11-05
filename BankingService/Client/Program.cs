using System;
using System.ServiceModel;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/BankingServices";

            using (ClientProxy proxy = new ClientProxy(binding, address))
            {
                if (proxy.OpenAccount("Mladen", "Milosevic"))
                    Console.WriteLine("Success! Account opened.");
                else
                    Console.WriteLine("Fail! Account was not opened.");
            }

            Console.ReadLine();
        }
    }
}
