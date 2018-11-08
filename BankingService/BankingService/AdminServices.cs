using CommonStuff.ClientContract;
using DatabaseLib;
using System;
using System.Threading;

namespace BankingService
{
    public class AdminServices : IAdminServices
    {
        public void CheckRequests()
        {
            // Ovde ce samo jednom da proveri, ne bi trebalo ovde staviti while, nego kod clienta

            // uzmemo sve zahteve i proveravamo na svakih 10 sekundi da li ima zastarelih
            Console.WriteLine("Checking database for old request ... ");

            var allRequests = RequestParser.GetRequests();
            foreach (var request in allRequests)
            {
                if (!request.IsProcessed)
                {
                    TimeSpan time = DateTime.Now.Subtract(request.DateAndTime);
                    if (time.Seconds > 2)
                    {
                        Console.WriteLine($"Request {request.ID} was older than 10 seconds. Deleting it ...");
                        RequestParser.DeleteRequest(request.ID);
                        Console.WriteLine($"Request {request.ID} deleted.");
                    }
                }
            }
            Thread.Sleep(500);
        }

        public void CreateDB()
        {
            RequestParser.CreateDB();
        }
    }
}
