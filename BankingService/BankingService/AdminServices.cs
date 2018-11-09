using AuditManager;
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
            // log successfull authentication
            string username = GetUsername();
            Audit.AuthenticationSuccess(username);

            // ako nema prava, vrati false
            if (!CheckAuthorization())
            {
                Audit.AuthorizationFailed(username, "CheckRequest", $"{username} is not Admin");
                return false;
            }

            // log successfull authorization
            Audit.AuthorizationSuccess(username, "CheckRequest");

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
                        Audit.DatabaseAction(username, $"Deleted request with ID: {request.ID}.");
                    }
                }
            }

            Thread.Sleep(1000);
            return true;
        }

        public bool CreateDB()
        {
            // log successfull authentication
            string username = GetUsername();
            Audit.AuthenticationSuccess(username);

            // ako nema prava, vrati false
            if (!CheckAuthorization())
            {
                Audit.AuthorizationFailed(username, "CreateDB", $"{username} is not Admin");
                return false;
            }

            // log successfull authorization
            Audit.AuthorizationSuccess(username, "CreateDB");

            RequestParser.CreateDB();
            Audit.DatabaseAction(username, "Created a database for requests.");

            return true;
        }

        private bool CheckAuthorization()
        {
            return ServiceSecurityContext.Current.PrimaryIdentity.Name.Split('=')[2].Contains("Admin");
        }

        private string GetUsername()
        {
            return ServiceSecurityContext.Current.PrimaryIdentity.Name.Split('=')[1].Split(',')[0];
        }
    }
}
