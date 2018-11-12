using AuditManager;
using CommonStuff.SectorContracts;
using DatabaseLib;
using System.Threading;

namespace BankingSectors
{
    public class CreditServices : ICreditServices, IStatusFree
    {
        private static bool IsFree = true;

        public bool IsItFree()
        {
            return IsFree;
        }

        public bool TakeLoan(string username,double amount)
        {
            IsFree = false;

            var account = AccountParser.GetAccount(username);
            if (account == null)
                return false;

            account.Credit += amount;

            //upis u JSON fajl (bazu)
            AccountParser.DeleteAccount(username);
            AccountParser.WriteAccount(account);
            Audit.DatabaseAction(username, $"Took a loan from the bank. Amount: {amount}.");

            Thread.Sleep(5000);
            IsFree = true;
            return true;
        }
    }
}
