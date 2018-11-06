using CommonStuff;
using DatabaseLib;
using DatabaseLib.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankingSectors
{
    public class AccountServices : IAccountServices,IStatusFree
    {
        public static bool IsFree = true; //
        

        
        public bool OpenAccount(string firstName, string lastName)
        {
            IsFree = false;
           

            try
            {
                Account newAccount = new Account() { ID = AccountParser.GetRandomID(), Balance = 0, Credit = 0 };
                User newUser = new User(firstName, lastName) { Account = newAccount };
                newAccount.Owner = newUser;

                // ako nije uspelo zbog neceg
                if (newUser.Account == null)
                    return false;


            }
            catch (Exception)
            {

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
