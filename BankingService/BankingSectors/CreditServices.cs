using CommonStuff.SectorContracts;
using System;

namespace BankingSectors
{
    public class CreditServices : ICreditServices, IStatusFree
    {
        public bool IsItFree()
        {
            throw new NotImplementedException();
        }

        public bool TakeLoan(string username,double amount)
        {
            throw new NotImplementedException();
        }
    }
}
