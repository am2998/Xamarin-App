
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Restart.Models
    {
  
    public class Risparmio : ReactiveObject
        {
        [JsonProperty (PropertyName = "id"), Reactive]
        public string ID { get; set; }

        [JsonProperty (PropertyName = "somma"), Reactive]
        public double Somma { get; set; }


        [JsonProperty (PropertyName = "nota"), Reactive]
        public string Nota { get; set; }

        [JsonProperty (PropertyName = "spesa"), Reactive]
        public string Spesa { get; set; }

        [JsonProperty (PropertyName = "chiave"), Reactive]
        public string Chiave { get; set; }
        }

    }