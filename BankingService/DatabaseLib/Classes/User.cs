using Newtonsoft.Json;

namespace DatabaseLib.Classes
{
    public class User
    {
        // da li korisnik treba da poseduje PIN ili lozinku jer ionako radimo preko sertifikata njihovu autentifikaciju?
        // kako treba da korisnik pristupi servisima, da li je potreba lozinka ili moze cisto da pozove?

        [JsonProperty(ReferenceLoopHandling = ReferenceLoopHandling.Ignore, IsReference = true)]
        public Account Account { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }


        public User(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Account = new Account(0, 0) { Owner = this };
        }

        // cisto da bude lakse testirati 
        public override string ToString()
        {
            return $"[UserInfo]: {FirstName} {LastName} - Balance: {Account.Balance} / Credit: {Account.Credit}";
        }

    }
}
