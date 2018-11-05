using Newtonsoft.Json;

namespace DatabaseLib.Classes
{
    public class Account
    {
        // kako treba da izgleda zapravo pravljenje novog naloga? 
        // sta od podataka je potrebno da bi klijent napravio nalog?

        [JsonProperty(ReferenceLoopHandling = ReferenceLoopHandling.Ignore, IsReference = true)]
        public User Owner { get; set; }
        public double Balance { get; set; }
        public double Credit { get; set; }

        public Account(double balance, double credit)
        {
            this.Balance = balance;
            this.Credit = credit;
        }
    }
}
