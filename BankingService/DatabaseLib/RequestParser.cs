using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using DatabaseLib.Classes;

namespace DatabaseLib
{
    public class RequestParser
    {
        private static string requestDbPath = @"C:\Users\Mladjo\Desktop\requests.json";     // ovo je samo test

        public static void WriteRequest(Request newRequest)
        {
            // ukoliko nije vec napravljen .json fajl, onda napravi
            if (!File.Exists(requestDbPath))
            {
                var fileCreate = File.CreateText(requestDbPath);
                fileCreate.Close();
            }

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

    }
}
