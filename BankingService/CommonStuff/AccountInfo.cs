using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonStuff
{
    public class AccountInfo
    {
        public bool DoesExsist { get; set; }
        public double Balance { get; set; }
        public double Credit { get; set; }

        public AccountInfo()
        {
            DoesExsist = false;
            Balance = 0;
            Credit = 0;
        }
    }
}
