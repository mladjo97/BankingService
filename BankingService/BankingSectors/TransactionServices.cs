using CommonStuff;
using CommonStuff.SectorContracts;
using DatabaseLib;
using DatabaseLib.Classes;
using System.Threading;

namespace BankingSectors
{
    public class TransactionServices : ITransactionServices, IStatusFree
    {
        public static bool IsFree = true; 

        public bool DoTransaction(string username,TransactionType type, double amount)
        {
            IsFree = false;

            Account userAccount = AccountParser.GetAccount(username);

            if (userAccount == null)
                return false;

            if(type == TransactionType.Deposit)
            {
                // logika Deposita - skidanje kredita ili nesto
                userAccount.Balance += amount;
                AccountParser.DeleteAccount(username);
                AccountParser.WriteAccount(userAccount);
            }
            else
            {
                // logika Withdrawa
                if(userAccount.Balance-amount >= 0)
                {
                    userAccount.Balance -= amount;
                    AccountParser.DeleteAccount(username);
                    AccountParser.WriteAccount(userAccount);
                }
                else
                {
                    return false; // jer nema dovoljno novca
                }
            }

            Thread.Sleep(5000);
            IsFree = true;
            return true;
        }

        public bool IsItFree()
        {
            return IsFree;
        }
    }
}
