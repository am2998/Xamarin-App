

using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using System;
using Xamarin.Forms.Maps;

namespace Restart.Models
    {


    public class Spesa : ReactiveObject
        {

        [JsonProperty (PropertyName = "id"), Reactive]
        public string ID { get; set; }

        [JsonProperty (PropertyName = "somma"), Reactive]
        public double Somma { get; set; }

        [JsonProperty (PropertyName = "categoria"), Reactive]
        public string Categoria { get; set; }

        [JsonProperty (PropertyName = "data"), Reactive]
        public DateTime Data { get; set; }

        [JsonProperty (PropertyName = "nota"), Reactive]
        public string Nota { get; set; }

        [JsonProperty (PropertyName = "completa"), Reactive]
        public bool Completa { get; set; }

        [JsonProperty (PropertyName = "chiave"), Reactive]
        public string Chiave { get; set; }

        [JsonProperty (PropertyName = "totaleRisparmi"), Reactive]
        public double TotaleRisparmi { get; set; }

        [JsonProperty (PropertyName = "differenza"), Reactive]

        public double Differenza { get; set; }

        /*  [JsonProperty (PropertyName = "latitudine"), Reactive]

          public double Latitudine { get; set; }

          [JsonProperty (PropertyName = "longitudine"), Reactive]
          public double Longitudine { get; set; }
        */
        [JsonProperty (PropertyName = "posizione"), Reactive]
        public Position posizione { get; set; }

        }
    }






