using CommonStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingService
{
    public class BankingServices : IUserServices
    {
        public void TestCall(int num)
        {
            Console.WriteLine($"{num}");
        }

    }
}
