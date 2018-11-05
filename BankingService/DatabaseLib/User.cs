using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLib
{
    public class User
    {
        // da li korisnik treba da poseduje PIN ili lozinku jer ionako radimo preko sertifikata njihovu autentifikaciju?
        // kako treba da korisnik pristupi servisima, da li je potreba lozinka ili moze cisto da pozove?

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Balance { get; set; }
        public double Credit { get; set; }

        public User(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Balance = 0;
            this.Credit = 0;
        }

        // cisto da bude lakse testirati 
        public override string ToString()
        {
            return $"[UserInfo]: {FirstName} {LastName} - Balance: {Balance} / Credit: {Credit}";
        }

    }
}
