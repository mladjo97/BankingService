using CommonStuff.ClientContract;
using DatabaseLib;
using System;
using System.ServiceModel;
using System.Threading;

namespace BankingService
{
    public class AdminServices : IAdminServices
    {
        public bool CheckRequests()
        {
            // ako nema prava, vrati false
            if (!CheckAuthorization())
                return false;

            // uzmemo sve zahteve i proveravamo na svakih 10 sekundi da li ima zastarelih
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
            return true;
        }

        public bool CreateDB()
        {
            // ako nema prava, vrati false
            if (!CheckAuthorization())
                return false;

            RequestParser.CreateDB();
            return true;
        }

        private bool CheckAuthorization()
        {
            return ServiceSecurityContext.Current.PrimaryIdentity.Name.Split('=')[2].Contains("Admin");
        }
    }
}
