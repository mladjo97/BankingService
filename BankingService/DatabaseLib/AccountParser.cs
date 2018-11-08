using DatabaseLib.Classes;
using Newtonsoft.Json;
using System;
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
            CreateDB();
            
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

        public static List<Account> GetAccounts()
        {
            CreateDB();

            // cita .json file
            var jsonData = File.ReadAllText(accountDbPath);

            // napravi listu Request objekata
            var list = JsonConvert.DeserializeObject<List<Account>>(jsonData) ?? new List<Account>();

            return list;
        }

        public static Account GetAccount(int id)
        {
            CreateDB();

            // cita .json file
            var jsonData = File.ReadAllText(accountDbPath);

            // napravi listu Request objekata
            var list = JsonConvert.DeserializeObject<List<Account>>(jsonData) ?? new List<Account>();

            Account request = new Account();

            foreach (var req in list)
                if (req.ID == id)
                    request = req;

            return request;
        }

        public static Account GetAccount(string owner)
        {
            CreateDB();

            // cita .json file
            var jsonData = File.ReadAllText(accountDbPath);

            // napravi listu Request objekata
            var list = JsonConvert.DeserializeObject<List<Account>>(jsonData) ?? new List<Account>();

            Account returnAcc = new Account();

            foreach (var account in list)
                if (account.Owner == owner)
                    returnAcc = account;

            return returnAcc;
        }

        public static void DeleteAccount(int id)
        {
            // ako nema db, nema ni brisanja
            if (!File.Exists(accountDbPath))
                return;

            // cita .json file
            var jsonData = File.ReadAllText(accountDbPath);

            // napravi listu Request objekata
            var list = JsonConvert.DeserializeObject<List<Account>>(jsonData) ?? new List<Account>();

            // ukloni zahtev
            foreach (var acc in list)
            {
                if (acc.ID == id)
                {
                    list.Remove(acc);
                    break;
                }
            }

            // zatim u Json pretvori listu, jer nam treba niz
            jsonData = JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);

            // i onda upise u .json
            File.WriteAllText(accountDbPath, jsonData);
        }

        public static void DeleteAccount(string owner)
        {
            // ako nema db, nema ni brisanja
            if (!File.Exists(accountDbPath))
                return;

            // cita .json file
            var jsonData = File.ReadAllText(accountDbPath);

            // napravi listu Request objekata
            var list = JsonConvert.DeserializeObject<List<Account>>(jsonData) ?? new List<Account>();

            // ukloni zahtev
            foreach (var acc in list)
            {
                if (acc.Owner == owner)
                {
                    list.Remove(acc);
                    break;
                }
            }

            // zatim u Json pretvori listu, jer nam treba niz
            jsonData = JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);

            // i onda upise u .json
            File.WriteAllText(accountDbPath, jsonData);
        }

        public static int GetRandomID()
        {
            var list = GetAccounts();

            bool existsID = false;
            Random rand = new Random();
            int randomID = rand.Next(1000, 10000);

            if (list.Count == 0)
                return randomID;

            do
            {
                existsID = false;
                randomID = rand.Next(1000, 10000);

                foreach (var acc in list)
                {
                    if (acc.ID == randomID)
                        existsID = true;
                }

            } while (existsID == true);

            return randomID;
        }

    }
}
