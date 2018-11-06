using DatabaseLib.Classes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace DatabaseLib
{
    public class AccountParser
    {
        private static string accountDbPath = @"C:\Users\Administrator\Desktop\BankingService\BankingService\accounts.json";     // ovo je samo test

        public static void CreateDB()
        {
            // ukoliko nije vec napravljen .json fajl, onda napravi
            if (!File.Exists(accountDbPath))
            {
                var fileCreate = File.CreateText(accountDbPath);
                fileCreate.Close();
            }
        }

        public static void WriteAccount(Account newAccount)
        {
            // ukoliko nije vec napravljen .json fajl, onda napravi
            if (!File.Exists(accountDbPath))
            {
                var fileCreate = File.CreateText(accountDbPath);
                fileCreate.Close();
            }

            // cita .json file
            var jsonData = File.ReadAllText(accountDbPath);

            // napravi listu Request objekata
            var list = JsonConvert.DeserializeObject<List<Account>>(jsonData) ?? new List<Account>();

            // doda novi request
            list.Add(newAccount);

            // zatim u Json pretvori listu, jer nam treba niz
            jsonData = JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);

            // i onda upise u .json
            File.WriteAllText(accountDbPath, jsonData);
        }

        public static List<Request> GetRequests()
        {
            CreateDB();

            // cita .json file
            var jsonData = File.ReadAllText(requestDbPath);

            // napravi listu Request objekata
            var list = JsonConvert.DeserializeObject<List<Request>>(jsonData) ?? new List<Request>();

            return list;
        }

    }
}
