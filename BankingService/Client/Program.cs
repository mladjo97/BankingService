using CertificationManager;
using CommonStuff;
using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            StartingMenu();
            
            //Console.ReadLine();
        }


        static void StartingMenu()
        {
            string srvCertCN = "bankingservice";

            bool check = true;
            string clientType;

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            /// uzmemo sertifikat servisa
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress adminAddress;
            EndpointAddress clientAddress;

            try
            {
                adminAddress = new EndpointAddress(new Uri("net.tcp://10.1.212.184:9998/AdminServices"), new X509CertificateEndpointIdentity(srvCert));
                clientAddress = new EndpointAddress(new Uri("net.tcp://10.1.212.184:9999/BankingServices"), new X509CertificateEndpointIdentity(srvCert));
            }
            catch(Exception e)
            {
                Console.WriteLine("[ERROR] You don't have the service certificate installed. \nMessage: {0}", e.Message);
                Console.WriteLine("Press any key to exit app...");
                Console.ReadLine();
                return;
            }


            while (check)
            {
                Console.WriteLine("Log in (as 'Admin' or 'User'):");
                clientType = Console.ReadLine();

                if (clientType.ToLower() == "admin")
                {
                    check = false;
                    string username = string.Empty;
                    X509Certificate2 clientCert;

                    do
                    {
                        Console.WriteLine("Enter username:");
                        username = Console.ReadLine();

                        // da li postoji sertifikat
                        clientCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, username);
                        if (clientCert != null)
                            break;
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("There is no username: {0}",username);
                        }

                    } while (true);

                    using (AdminProxy adminProxy = new AdminProxy(binding, adminAddress, clientCert))
                    {
                        bool secondOut = true;
                        do
                        {
                            int operation = AdminMenu();

                            if (operation == 1)
                            {
                                if (adminProxy.CreateDB())
                                {
                                    Console.Clear();
                                    Console.WriteLine("DB was created.");
                                }
                                else
                                {                                    
                                    Console.WriteLine("DB was not created.");
                                }
                            }
                            else if(operation == 2)
                            {
                                while (true)
                                {
                                    // ako je enter break
                                    if(Console.KeyAvailable)
                                    {
                                        Console.ReadKey();
                                        break;
                                    }

                                    try
                                    {
                                        if (!adminProxy.CheckRequests())
                                        {                                            
                                            Console.WriteLine("You cant check requests.");
                                            break;
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Error: {0}", e.Message);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                secondOut = false;
                                Console.WriteLine("Press any key to close the program...");
                                Console.ReadKey();
                                return;
                            }
                        } while (secondOut);

                    }

                }
                else if (clientType.ToLower() == "user")
                {

                        check = false;
                        //string clientAddress = "net.tcp://localhost:9999/BankingServices";

                        // Ovo cemo menjati kad budemo radili sa sertifikatom
                        string username = string.Empty;
                        X509Certificate2 clientCert;

                        do
                        {
                            Console.WriteLine("Enter username:");
                            username = Console.ReadLine();

                            // da li postoji sertifikat
                            clientCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, username);
                            if (clientCert != null)
                                break;
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("There is no username: {0}", username);
                            }

                    } while (true);

                    while (true)
                    {

                        int operation = OperationMenu();

                        if (operation == 1)
                        {
                            try
                            {
                                using (ClientProxy proxy = new ClientProxy(binding, clientAddress, clientCert))
                                {

                                    // otvori racun                
                                    if (proxy.OpenAccount(username))
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Success! Account opened.");
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Fail! Account was not opened.");
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Error, {0}", e.Message);
                            }

                        }
                        else if (operation == 2)
                        {
                            try
                            {
                                using (ClientProxy proxy = new ClientProxy(binding, clientAddress, clientCert))
                                {
                                    // Credit
                                    Console.WriteLine("Enter the amount you want:");
                                    string amount = Console.ReadLine();

                                    if (proxy.TakeLoan(username, double.Parse(amount)))
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Success! Loan taken.");

                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Fail! Loan was not approved.");
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Error, {0}", e.Message);
                            }
                        }
                        else if (operation == 0)
                        {
                            Console.WriteLine("Press any key to close the program...");
                            return;
                        }
                        else if(operation == 9)
                        {
                            try
                            {
                                using (ClientProxy proxy = new ClientProxy(binding, clientAddress, clientCert))
                                {
                                    AccountInfo acc = proxy.GetAccountInfo(username);
                                    if (acc == null)
                                    {
                                        Console.WriteLine("Fail! You dont have the permision");
                                    }

                                    // otvori racun                
                                    else if (acc.DoesExist)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Account Info:\n Username: {0} \nCredit: {1} \nBalance: {2}", username, acc.Credit, acc.Balance);
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Fail! Account does not exist.");
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Console.Clear();
                                Console.WriteLine("You don't have the verified CA for this service.");
                                Console.WriteLine("Error: {0}",e.Message);
                            }
                            
                        }

                        else
                        {
                            using (ClientProxy proxy = new ClientProxy(binding, clientAddress, clientCert))
                            {
                                // Transakcija ( Uplata / Isplata)
                                Console.WriteLine("Enter the amount you want:");
                                string amount = Console.ReadLine();
                                TransactionType transactionType;

                                if (operation == 4)
                                    transactionType = TransactionType.Deposit;
                                else
                                    transactionType = TransactionType.Withdrawal;

                                if (proxy.DoTransaction(username, transactionType, double.Parse(amount)))
                                {
                                    Console.Clear();
                                    Console.WriteLine("Success! The transaction is done");

                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine("Fail! You were unable to do the Transaction");
                                }
                            }
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
            bool secondOut = true;
            do
            {
                Console.WriteLine("Choose the operation:");
                Console.WriteLine("1. OpenAccount \n2.Take loan \n3.Make transactions\n9.Show info about the account\n0.Close program");

               opp = Console.ReadLine();
                if(opp =="1" || opp == "2" || opp =="0" || opp == "9")
                {
                    break;
                }
                else if(opp == "3")
                {
                    do
                    {
                        Console.WriteLine("Enter '4' for Deposit or 5 for Withdrawal");
                        opp = Console.ReadLine();

                        if (opp == "4" || opp == "5")
                        {
                            secondOut = false;
                            break;
                        }
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
            while (secondOut);

            return int.Parse(opp);
        }

        static int AdminMenu()
        {
            string opp = string.Empty;
            do
            {
                Console.WriteLine("Choose operation:\n1.Create database\n2.Check if there are old requests\n0.To close the program");
                opp = Console.ReadLine();

                if (opp == "1" || opp == "2" || opp =="0")
                {
                    break;
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