using Newtonsoft.Json;

namespace DatabaseLib.Classes
{
    public class Account
    {
        // kako treba da izgleda zapravo pravljenje novog naloga? 
        // sta od podataka je potrebno da bi klijent napravio nalog?

        [JsonProperty(ReferenceLoopHandling = ReferenceLoopHandling.Ignore, IsReference = true)]
        public string Owner { get; set; }
        public int ID { get; set; }
        public double Balance { get; set; }
        public double Credit { get; set; }

        public Account()
        {
            
            this.Balance = 0;
            this.Credit = 0;
        }

        public Account(string owner)
        {
            Owner = owner;
            this.Balance = 0;
            this.Credit = 0;
        }

        public Account(string owner ,double balance, double credit)
        {
            Owner = owner;
            this.Balance = balance;
            this.Credit = credit;
        }

        public override string ToString()
        {
            return $"[AccountInfo] Owner: {Owner}  / Balance: {Balance} / Credit: {Credit}";
        }
    }
}
