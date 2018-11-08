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
            //Console.WriteLine("Checking database for old request ... ");

            var allRequests = RequestParser.GetRequests();
            foreach (var request in allRequests)
            {
                if (!request.IsProcessed && !request.InProcess)
                {
                    TimeSpan time = DateTime.Now.Subtract(request.DateAndTime);

                    Console.WriteLine($"{request.ID} - {request.User} time: {time.Seconds}");

                    if (time.Seconds > 5)
                    {
                        Console.WriteLine($"Request {request.ID} was older than 5 seconds. Deleting it ...");
                        RequestParser.DeleteRequest(request.ID);
                        Console.WriteLine($"Request {request.ID} deleted.");
                    }
                }
            }

            Thread.Sleep(1000);
        }

        public void CreateDB()
        {
            RequestParser.CreateDB();
        }
    }
}
