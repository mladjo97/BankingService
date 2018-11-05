using CommonStuff;
using DatabaseLib;
using DatabaseLib.Classes;
using System;

namespace BankingService
{
    public class BankingServices : IUserServices
    {
        public bool OpenAccount(string firstName, string lastName)
        {
            // posto se u konstruktoru User napravi novi account - mozda treba izmeniti
            User newUser = new User(firstName, lastName);

            // ako nije uspelo zbog neceg
            if (newUser.Account == null)
                return false;

            Request req = new Request();
            req.DateAndTime = DateTime.Now;
            req.Action = RequestAction.OpenAccount;
            req.User = newUser;
            req.IsProcessed = false;    // ovo posle mozemo promeniti iz sektora direktno ili ovde kad vrati odgovor

            RequestParser.WriteRequest(req);

            return true;
        }
    }
}
