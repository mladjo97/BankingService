using CommonStuff;
using DatabaseLib;
using DatabaseLib.Classes;
using System;

namespace BankingService
{
    public class BankingServices : IUserServices, IAdminServices
    {
        public bool OpenAccount(string firstName, string lastName)
        {
            // posto se u konstruktoru User napravi novi account - mozda treba izmeniti
            User newUser = new User(firstName, lastName);

            // ako nije uspelo zbog neceg
            if (newUser.Account == null)
                return false;

            // napravimo novi zahtev
            Request req = new Request();
            req.ID = RequestParser.GetRandomID();
            req.DateAndTime = DateTime.Now;
            req.Action = RequestAction.OpenAccount;
            req.User = newUser;
            req.IsProcessed = false;    // ovo posle mozemo promeniti iz sektora direktno ili ovde kad vrati odgovor

            RequestParser.WriteRequest(req);

            // ovo ispod treba ITSector da radi ali cisto test
            // Koja je logika za novog korisnika ? imamo li login stranicu ili treba preko sertifikata da proverimo

            Account newAccount = new Account(0, 0) { Owner = newUser };

            AccountParser.WriteAccount(newAccount);

            RequestParser.MarkProcessed(req.ID);    // ovo moramo nekako sklopiti kad napravimo odvojen servise radi ID

            return true;
        }

        public bool TakeLoan(double amount)
        {
            Request req = new Request();
            req.ID = RequestParser.GetRandomID();
            req.DateAndTime = DateTime.Now;
            req.Action = RequestAction.TakeLoan;
            req.User = new User("Mladen", "Milosevic");
            req.IsProcessed = false;

            RequestParser.WriteRequest(req);

            return true;
        }

        public bool DoTransaction(TransactionType type, double amount)
        {
            Request req = new Request();
            req.ID = RequestParser.GetRandomID();
            req.DateAndTime = DateTime.Now;
            req.User = new User("Mladen", "Milosevic");
            req.IsProcessed = false;

            if (type == TransactionType.Deposit)
                req.Action = RequestAction.Deposit;
            else
                req.Action = RequestAction.Withdrawal;

            RequestParser.WriteRequest(req);

            return true;
        }

    }
}
