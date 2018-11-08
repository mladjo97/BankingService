using CommonStuff;
using System;
using System.ServiceModel;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            StartingMenu();
            
            Console.ReadLine();

        }


        static void StartingMenu()
        {
            bool check = true;
            string clientType;

            NetTcpBinding binding = new NetTcpBinding();

            while (check)
            {
                Console.WriteLine("Log in (as 'Admin' or 'User'):");
                clientType = Console.ReadLine();

                if (clientType.ToLower() == "admin")
                {
                    check = false;
                    string adminAddress = "net.tcp://localhost:9998/AdminServices";

                    // Ovo cemo menjati kad budemo radili sa sertifikatom
                    Console.WriteLine("Enter username:");
                    string username = Console.ReadLine();

                    using (AdminProxy adminProxy = new AdminProxy(binding, adminAddress))
                    {
                        adminProxy.CreateDB();

                        while (true)
                        {
                            // ako je enter break
                            try
                            {
                                adminProxy.CheckRequests();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Error: {0}",e);
                                break;
                            }
                        }
                    }

                }
                else if (clientType.ToLower() == "user")
                {
                    check = false;
                    string clientAddress = "net.tcp://localhost:9999/BankingServices";

                    // Ovo cemo menjati kad budemo radili sa sertifikatom
                    Console.WriteLine("Enter username:");
                    string username = Console.ReadLine();

                    int operation = OperationMenu();

                    if (operation == 1)
                    {
                        using (ClientProxy proxy = new ClientProxy(binding, clientAddress))
                        {
                            // otvori racun                
                            if (proxy.OpenAccount(username))
                                Console.WriteLine("Success! Account opened.");
                            else
                                Console.WriteLine("Fail! Account was not opened.");
                        }
                    }
                    else if (operation == 2)
                    {
                        using (ClientProxy proxy = new ClientProxy(binding, clientAddress))
                        {
                            // Credit
                            Console.WriteLine("Enter the amount you want:");
                            string amount = Console.ReadLine();
                            if (proxy.TakeLoan(username, int.Parse(amount)))
                                Console.WriteLine("Success! Loan taken.");
                            else
                                Console.WriteLine("Fail! Loan was not approved.");
                        }
                    }

                    else 
                    {
                        using (ClientProxy proxy = new ClientProxy(binding, clientAddress))
                        {
                            // Transakcija ( Uplata / Isplata)
                            Console.WriteLine("Enter the amount you want:");
                            string amount = Console.ReadLine();
                            TransactionType transactionType;

                            if (operation == 4)
                                transactionType = TransactionType.Deposit;
                            else
                                transactionType = TransactionType.Withdrawal;

                            if (proxy.DoTransaction(username, transactionType,int.Parse(amount)))
                                Console.WriteLine("Success! The transaction is done");
                            else
                                Console.WriteLine("Fail! You were unable to do the Transaction");
                        }
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("You have to log in as Admin or User, try it again !");

                }
            }
        }

        static int OperationMenu()
        {
            string opp;
            do
            {
                Console.WriteLine("Choose the operation:");
                Console.WriteLine("1. OpenAccount \n2.Take loan \n3.Make transactions");

               opp = Console.ReadLine();
                if(opp =="1" || opp == "2" )
                {
                    break;
                }
                else if(opp == "3")
                {
                    do
                    {
                        Console.WriteLine("Enter '4' for Deposit or 5 for Withdrawal");
                        opp = Console.ReadLine();

                        if (opp == "4" || opp== "5")
                            break;
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Error, you didn't enter the right number...Try it again");
                        }
                    }
                    while (true);
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Error, you didn't enter the right number...Try it again");
                }
            }
            while (true);

            return int.Parse(opp);
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