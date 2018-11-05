using CommonStuff;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace DatabaseLib.Classes

{
    public class Request
    {
        public User User { get; set; }
        public DateTime DateAndTime { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public RequestAction Action { get; set; }
        public bool IsProcessed { get; set; }
        
        public Request() { }
    }
}
