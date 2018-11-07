using CommonStuff.SectorContracts;
using DatabaseLib;
using DatabaseLib.Classes;
using System;
using System.Threading;

namespace BankingSectors
{
    public class AccountServices : IAccountServices,IStatusFree
    {
        public static bool IsFree = true; //
        
        public bool OpenAccount(string username)
        {            
            IsFree = false;
            Thread.Sleep(10000);

            try
            {
                Account newAccount = new Account(username) { ID = AccountParser.GetRandomID(), Balance = 0, Credit = 0 };
                AccountParser.WriteAccount(newAccount);

            }
            catch (Exception)
            {
                return false;
            }
           
            
            IsFree = true;

            return true; // proslo je sve kako treba
        }

        public bool IsItFree() //da li je sektor slobodan
        {
            return IsFree;
        }
    }
}
