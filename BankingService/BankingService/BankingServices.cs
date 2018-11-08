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
        private static Queue transactionQueue = new Queue();
        private static Queue creditQueue = new Queue();

        public bool OpenAccount(string username)
        {
            
            // napravimo novi zahtev
            Request req = new Request();
            req.ID = RequestParser.GetRandomID();
            req.DateAndTime = DateTime.Now;
            req.Action = RequestAction.OpenAccount;
            req.User = username;
            req.IsProcessed = false;    // ovo posle mozemo promeniti iz sektora direktno ili ovde kad vrati odgovor
            req.InProcess = false;

            RequestParser.WriteRequest(req);
            
            // postavimo u red korisnika
            accountQueue.Enqueue(username);            

            while (true)
            {              
                Console.WriteLine("AccountSector is not available currently.");

                string next = (string)accountQueue.Peek();
                bool free = sectorProxy.AccountProxy.IsItFree();

                Console.WriteLine($"{username}: free:{free} / next:{next}");

                if (free && next == username)
                    break;

                Thread.Sleep(1000);
            }

            // da li postoji zahtev 
            bool stillExists = RequestExists(req.ID);
            if (!stillExists)
                return false;

            // trenutno obradjujemo, ne brisi
            RequestParser.MarkInProcess(req.ID);

            // posaljemo ga kad se oslobodi i dodje njegov red
            bool accountResult = sectorProxy.AccountProxy.OpenAccount(username);
            accountQueue.Dequeue();

            RequestParser.MarkProcessed(req.ID);    // ovo moramo nekako sklopiti kad napravimo odvojen servise radi ID

            return accountResult;
        }

        public bool TakeLoan(string username, double amount)
        {
            Request req = new Request();
            req.ID = RequestParser.GetRandomID();
            req.DateAndTime = DateTime.Now;
            req.Action = RequestAction.TakeLoan;
            req.User = username;
            req.IsProcessed = false;
            req.InProcess = false;

            RequestParser.WriteRequest(req);

            // postavimo u red korisnika
            creditQueue.Enqueue(username);

            while (true)
            {
                Thread.Sleep(1000);
                Console.WriteLine("TransactionSector is not available currently.");

                string next = (string)accountQueue.Peek();
                bool free = sectorProxy.CreditProxy.IsItFree();

                Console.WriteLine($"{username}: free:{free} / next:{next}");

                if (free && next == username)
                    break;
            }

            // da li postoji zahtev 
            bool stillExists = RequestExists(req.ID);
            if (!stillExists)
                return false;

            // posaljemo ga kad se oslobodi i dodje njegov red
            bool creditResult = sectorProxy.CreditProxy.TakeLoan(username, amount);
            creditQueue.Dequeue();

            RequestParser.MarkProcessed(req.ID);    // ovo moramo nekako sklopiti kad napravimo odvojen servise radi ID

            return creditResult;
        }

        public bool DoTransaction(string username,TransactionType type, double amount)
        {
            // upise neobradjen zahtev
            Request req = new Request();
            req.ID = RequestParser.GetRandomID();
            req.DateAndTime = DateTime.Now;
            req.User = username;
            req.IsProcessed = false;
            req.InProcess = false;

            if (type == TransactionType.Deposit)
                req.Action = RequestAction.Deposit;
            else
                req.Action = RequestAction.Withdrawal;

            RequestParser.WriteRequest(req);

            // postavimo u red korisnika
            creditQueue.Enqueue(username);

            while (true)
            {
                Thread.Sleep(1000);
                Console.WriteLine("TransactionSector is not available currently.");

                string next = (string)accountQueue.Peek();
                bool free = sectorProxy.TransactionProxy.IsItFree();

                Console.WriteLine($"{username}: free:{free} / next:{next}");

                if (free && next == username)
                    break;
            }

            // da li postoji zahtev 
            bool stillExists = RequestExists(req.ID);
            if (!stillExists)
                return false;

            // posaljemo ga kad se oslobodi i dodje njegov red
            bool transactionResult = sectorProxy.TransactionProxy.DoTransaction(username, type, amount);
            transactionQueue.Dequeue();

            RequestParser.MarkProcessed(req.ID);    // posto je on fakticki obradjen, al moze li biti neuspelo?

            return transactionResult;
        }

        private bool RequestExists(int reqID)
        {
            var req = RequestParser.GetRequest(reqID);

            if (req.ID == 0)
                return false;

            return true;
        }

    }
}
