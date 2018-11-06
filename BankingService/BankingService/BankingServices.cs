using CommonStuff;
using DatabaseLib;
using DatabaseLib.Classes;
using System;

namespace BankingService
{
    public class BankingServices : IUserServices
    {

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


            RequestParser.MarkProcessed(req.ID);    // ovo moramo nekako sklopiti kad napravimo odvojen servise radi ID

            return true;
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

            return true;
        }

    }
}
