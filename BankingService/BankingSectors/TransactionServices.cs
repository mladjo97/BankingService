using AuditManager;
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

        public bool DoTransaction(string username, TransactionType type, double amount)
        {
            IsFree = false;

            Account userAccount = AccountParser.GetAccount(username);

            if (userAccount == null)
                return false;

            if(type == TransactionType.Deposit)
            {
                // ako korisnik ima kredita onda mu od svake uplate skidamo 50%
                if (userAccount.Credit > 0)
                {
                    double percent = amount * 0.5;

                    if (userAccount.Credit - percent < 0)
                    {
                        userAccount.Balance += amount - userAccount.Credit;
                        userAccount.Credit = 0;
                    }
                    else
                    {
                        userAccount.Credit -= percent;
                        userAccount.Balance += amount - percent;
                    }
                }
                else
                {
                    userAccount.Balance += amount;
                }
 
                AccountParser.DeleteAccount(username);
                AccountParser.WriteAccount(userAccount);

                Audit.DatabaseAction(username, $"Deposited {amount} into his/hers account.");
            }
            else
            {
                // logika Withdrawa
                if(userAccount.Balance-amount >= 0)
                {
                    userAccount.Balance -= amount;
                    AccountParser.DeleteAccount(username);
                    AccountParser.WriteAccount(userAccount);
                    Audit.DatabaseAction(username, $"Withdraw {amount} from his/hers account.");
                }
                else
                {
                    Thread.Sleep(5000);
                    IsFree = true;
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
