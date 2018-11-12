using AuditManager;
using CommonStuff;
using CommonStuff.ClientContract;
using DatabaseLib;
using DatabaseLib.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;
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
            // log successfull authentication
            Audit.AuthenticationSuccess(username);

            // ako nema prava, vrati false
            if (!CheckAuthorization())
            {
                Audit.AuthorizationFailed(username, "OpenAccount", $"{username} is not User");
                return false;
            }

            // log successfull authorization
            Audit.AuthorizationSuccess(username, "OpenAccount");

            // napravimo novi zahtev
            Request req = new Request();
            req.ID = RequestParser.GetRandomID();
            req.DateAndTime = DateTime.Now;
            req.Action = RequestAction.OpenAccount;
            req.User = username;
            req.IsProcessed = false;
            req.InProcess = false;

            RequestParser.WriteRequest(req);

            // postavimo u red korisnika
            accountQueue.Enqueue(username);

            while (true)
            {
                string next = (string)accountQueue.Peek();
                bool free = sectorProxy.AccountProxy.IsItFree();

                Console.WriteLine($"{username} connected to OpenAccount / Free: {free}, Next: {next}");

                if (free && next == username)
                    break;

                Console.WriteLine("AccountSector is not available currently.");
                Thread.Sleep(1000);
            }

            // da li postoji zahtev 
            bool stillExists = RequestExists(req.ID);
            if (!stillExists)
                return false;

            // trenutno obradjujemo, ne brisi
            RequestParser.MarkInProcess(req.ID);

            // posaljemo ga kad se oslobodi i dodje njegov red
            accountQueue.Dequeue();
            bool accountResult = sectorProxy.AccountProxy.OpenAccount(username);

            RequestParser.MarkProcessed(req.ID);
            RequestParser.FinishProcess(req.ID);

            return accountResult;
        }

        public bool TakeLoan(string username, double amount)
        {
            // log successfull authentication
            Audit.AuthenticationSuccess(username);

            // ako nema prava, vrati false
            if (!CheckAuthorization())
            {
                Audit.AuthorizationFailed(username, "TakeLoan", $"{username} is not User");
                return false;
            }

            // log successfull authorization
            Audit.AuthorizationSuccess(username, "TakeLoan");

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
                string next = (string)creditQueue.Peek();
                bool free = sectorProxy.CreditProxy.IsItFree();

                Console.WriteLine($"{username} connected to TakeLoan / Free: {free}, Next: {next}");

                if (free && next == username)
                    break;

                Console.WriteLine("CreditSector is not available currently.");
                Thread.Sleep(1000);
            }

            // da li postoji zahtev 
            bool stillExists = RequestExists(req.ID);
            if (!stillExists)
                return false;

            // trenutno obradjujemo, ne brisi
            RequestParser.MarkInProcess(req.ID);

            // posaljemo ga kad se oslobodi i dodje njegov red
            creditQueue.Dequeue();
            bool creditResult = sectorProxy.CreditProxy.TakeLoan(username, amount);

            RequestParser.MarkProcessed(req.ID);
            RequestParser.FinishProcess(req.ID);

            return creditResult;
        }

        public bool DoTransaction(string username, TransactionType type, double amount)
        {
            // log successfull authentication
            Audit.AuthenticationSuccess(username);

            // ako nema prava, vrati false
            if (!CheckAuthorization())
            {
                Audit.AuthorizationFailed(username, "DoTransaction", $"{username} is not User");
                return false;
            }

            // log successfull authorization
            Audit.AuthorizationSuccess(username, "DoTransaction");

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
            transactionQueue.Enqueue(username);

            while (true)
            {
                string next = (string)transactionQueue.Peek();
                bool free = sectorProxy.TransactionProxy.IsItFree();

                Console.WriteLine($"{username} connected to DoTransaction / Free: {free}, Next: {next}");

                if (free && next == username)
                    break;

                Console.WriteLine("TransactionSector is not available currently.");
                Thread.Sleep(1000);
            }

            // da li postoji zahtev 
            bool stillExists = RequestExists(req.ID);
            if (!stillExists)
                return false;

            // trenutno obradjujemo, ne brisi
            RequestParser.MarkInProcess(req.ID);

            // posaljemo ga kad se oslobodi i dodje njegov red
            transactionQueue.Dequeue();
            bool transactionResult = sectorProxy.TransactionProxy.DoTransaction(username, type, amount);

            RequestParser.MarkProcessed(req.ID);
            RequestParser.FinishProcess(req.ID);

            return transactionResult;
        }

        public AccountInfo GetAccountInfo(string username)
        {
            // log successfull authentication
            Audit.AuthenticationSuccess(username);

            // ako nema prava, vrati false
            if (!CheckAuthorization())
            {
                Audit.AuthorizationFailed(username, "GetAccountInfo", $"{username} is not User");
                return null;
            }

            // log successfull authorization
            Audit.AuthorizationSuccess(username, "GetAccountInfo");

            // napravimo novi zahtev
            Request req = new Request();
            req.ID = RequestParser.GetRandomID();
            req.DateAndTime = DateTime.Now;
            req.Action = RequestAction.AccountInfo;
            req.User = username;
            req.IsProcessed = true;
            req.InProcess = false;

            RequestParser.WriteRequest(req);


            AccountInfo account = new AccountInfo();
            List<Account> accounts = AccountParser.GetAccounts();
            foreach (Account item in accounts)
            {
                if (item.Owner == username)
                {
                    account.DoesExist = true;
                    account.Balance = item.Balance;
                    account.Credit = item.Credit;
                    break;
                }
            }

            return account;
        }

        private bool RequestExists(int reqID)
        {
            var req = RequestParser.GetRequest(reqID);

            if (req.ID == 0)
                return false;

            return true;
        }

        private bool CheckAuthorization()
        {
            return ServiceSecurityContext.Current.PrimaryIdentity.Name.Split('=')[2].Contains("User");
        }

    }
}
