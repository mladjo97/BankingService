using CommonStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSectors
{
    public class TransactionServices : ITransactionServices
    {
        public bool DoTransaction(string username,TransactionType type, double amount)
        {
            throw new NotImplementedException();
        }
    }
}
