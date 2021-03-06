﻿using AuditManager;
using CommonStuff.SectorContracts;
using DatabaseLib;
using DatabaseLib.Classes;
using System;
using System.Threading;

namespace BankingSectors
{
    public class AccountServices : IAccountServices, IStatusFree
    {
        public static bool IsFree = true;

        public bool OpenAccount(string username)
        {
            IsFree = false;

            try
            {
                Account newAccount = new Account(username) { ID = AccountParser.GetRandomID(), Balance = 0, Credit = 0 };

                // ako postoji nalog, nema potrebe novi da pravimo
                var accounts = AccountParser.GetAccounts();
                foreach (var acc in accounts)
                    if (acc.Owner == username)
                    {
                        Thread.Sleep(5000);
                        IsFree = true;
                        return false;
                    }

                AccountParser.WriteAccount(newAccount);
                Audit.DatabaseAction(username, "Opened a new account.");
            }
            catch (Exception)
            {
                IsFree = true;
                return false;
            }

            Thread.Sleep(5000);
            IsFree = true;            

            return true; // proslo je sve kako treba
        }

        public bool IsItFree() //da li je sektor slobodan
        {
            return IsFree;
        }
    }
}
