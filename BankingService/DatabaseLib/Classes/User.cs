using Newtonsoft.Json;

namespace DatabaseLib.Classes
{
    public class User
    {
        [JsonProperty(ReferenceLoopHandling = ReferenceLoopHandling.Ignore, IsReference = true)]
        public Account Account { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }


        public User(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;            
        }

        // cisto da bude lakse testirati 
        public override string ToString()
        {
            return $"[UserInfo]: {FirstName} {LastName} - Balance: {Account.Balance} / Credit: {Account.Credit}";
        }

    }
}
