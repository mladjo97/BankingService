using CommonStuff;
using CommonStuff.ClientContract;
using DatabaseLib;
using DatabaseLib.Classes;
using System;
using System.Collections;
using System.Threading;

namespace BankingService
{
    public class BankingServices : IUserServices
    {
        private SectorProxy sectorProxy = new SectorProxy();
        private static Queue accountQueue = new Queue();

        public bool OpenAccount(string username)
        {
            
            // napravimo novi zahtev
            Request req = new Request();
            req.ID = RequestParser.GetRandomID();
            req.DateAndTime = DateTime.Now;
            req.Action = RequestAction.OpenAccount;
            req.User = username;
            req.IsProcessed = false;    // ovo posle mozemo promeniti iz sektora direktno ili ovde kad vrati odgovor

            RequestParser.WriteRequest(req);

            //pozivati proxy i proveravati u while()

            accountQueue.Enqueue(username);            

            while (true)
            {
                Thread.Sleep(1000);

                Console.WriteLine("TransactionSector is not available currently.");

                string next = (string)accountQueue.Peek();
                bool free = sectorProxy.AccountProxy.IsItFree();

                Console.WriteLine($"{username}: free:{free} / next:{next}");

                if (free && next == username)
                    break;
                
            }            

            bool accountResult = sectorProxy.AccountProxy.OpenAccount(username);
            accountQueue.Dequeue();

            RequestParser.MarkProcessed(req.ID);    // ovo moramo nekako sklopiti kad napravimo odvojen servise radi ID

            return accountResult;
        }

        public bool TakeLoan(string username,double amount)
        {
            Request req = new Request();
            req.ID = RequestParser.GetRandomID();
            req.DateAndTime = DateTime.Now;
            req.Action = RequestAction.TakeLoan;
            req.User = username;
            req.IsProcessed = false;

            RequestParser.WriteRequest(req);

            return true;
        }

        public bool DoTransaction(string username,TransactionType type, double amount)
        {
            // upise neobradjen zahtev
            Request req = new Request();
            req.ID = RequestParser.GetRandomID();
            req.DateAndTime = DateTime.Now;
            req.User = username;
            req.IsProcessed = false;

            if (type == TransactionType.Deposit)
                req.Action = RequestAction.Deposit;
            else
                req.Action = RequestAction.Withdrawal;

            RequestParser.WriteRequest(req);

            // pozivanje transaction sektor servisa
            while (!sectorProxy.TransactionProxy.IsItFree())
            {
                Console.WriteLine("TransactionSector is not available currently.");
                Thread.Sleep(2000);
            }

            bool transactionResult = sectorProxy.TransactionProxy.DoTransaction(username, type, amount);

            RequestParser.MarkProcessed(req.ID);    // posto je on fakticki obradjen, al moze li biti neuspelo?

            return transactionResult;
        }

    }
}
