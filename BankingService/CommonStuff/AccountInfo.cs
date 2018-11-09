using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonStuff
{
    public class AccountInfo
    {
        public bool DoesExist { get; set; }
        public double Balance { get; set; }
        public double Credit { get; set; }

        public AccountInfo()
        {
            DoesExist = false;
            Balance = 0;
            Credit = 0;
        }
    }
}
