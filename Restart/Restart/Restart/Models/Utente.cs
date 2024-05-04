using Newtonsoft.Json;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restart.Models
    {
    public class Utente
        {
        [JsonProperty (PropertyName = "id"), Reactive]
        public string ID { get; set; }

        [JsonProperty (PropertyName = "somma"), Reactive]
        public string Email { get; set; }
        
        [JsonProperty (PropertyName = "password"), Reactive]
        public string Password { get; set; }

        }
    }
