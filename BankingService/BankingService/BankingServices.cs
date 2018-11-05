using CommonStuff;
using DatabaseLib;
using System;

namespace BankingService
{
    public class BankingServices : IUserServices
    {
        public void TestCall(int num)
        {
            Console.WriteLine($"{num}");

            Request newRequest = new Request();
            newRequest.User = new User("Mladen", "Milosevic");
            newRequest.DateAndTime = DateTime.Now;
            newRequest.IsProcessed = false;
            newRequest.Action = RequestAction.OpenAccount;

            RequestParser.WriteRequest(newRequest);

        }

    }
}
