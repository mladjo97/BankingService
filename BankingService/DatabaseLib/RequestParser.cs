using DatabaseLib.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DatabaseLib
{
    public class RequestParser
    {
        private static string requestDbPath = @"C:\Users\Mladjo\Desktop\requests.json";     // ovo je samo test

        public static void CreateDB()
        {
            // ukoliko nije vec napravljen .json fajl, onda napravi
            if (!File.Exists(requestDbPath))
            {
                var fileCreate = File.CreateText(requestDbPath);
                fileCreate.Close();
            }
        }

        public static void WriteRequest(Request newRequest)
        {
            CreateDB();

            // cita .json file
            var jsonData = File.ReadAllText(requestDbPath);

            // napravi listu Request objekata
            var list = JsonConvert.DeserializeObject<List<Request>>(jsonData) ?? new List<Request>();

            // doda novi request
            list.Add(newRequest);

            // zatim u Json pretvori listu, jer nam treba niz
            jsonData = JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);

            // i onda upise u .json
            File.WriteAllText(requestDbPath, jsonData);
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


        public static Request GetRequest(int id)
        {
            CreateDB();

            // cita .json file
            var jsonData = File.ReadAllText(requestDbPath);

            // napravi listu Request objekata
            var list = JsonConvert.DeserializeObject<List<Request>>(jsonData) ?? new List<Request>();

            Request request = new Request();

            foreach (var req in list)
                if (req.ID == id)
                    request = req;

            return request;
        }


        public static void DeleteRequest(int id)
        {
            // ako nema db, nema ni brisanja
            if (!File.Exists(requestDbPath))
                return;

            // cita .json file
            var jsonData = File.ReadAllText(requestDbPath);

            // napravi listu Request objekata
            var list = JsonConvert.DeserializeObject<List<Request>>(jsonData) ?? new List<Request>();

            // ukloni zahtev
            foreach (var req in list)
            {
                if (req.ID == id)
                {
                    list.Remove(req);
                    break;
                }
            }

            // zatim u Json pretvori listu, jer nam treba niz
            jsonData = JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);

            // i onda upise u .json
            File.WriteAllText(requestDbPath, jsonData);
        }


        public static void MarkProcessed(int id)
        {
            var request = GetRequest(id);

            request.IsProcessed = true;

            DeleteRequest(id);
            WriteRequest(request);
        }


        public static int GetRandomID()
        {
            var list = GetRequests();

            bool existsID = false;
            Random rand = new Random();
            int randomID = rand.Next(1000, 10000);

            if (list.Count == 0)
                return randomID;

            do
            {
                existsID = false;
                randomID = rand.Next(1000, 10000);

                foreach (var req in list)
                {
                    if (req.ID == randomID)
                        existsID = true;
                }

            } while (existsID == true);

            return randomID;
        }

    }
}
