using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;


namespace Restart.Models
    {
    public class Categorie : ReactiveObject
        {

        [JsonProperty (PropertyName = "id"), Reactive]
        public string Tipo { get; set; }

        [JsonProperty (PropertyName = "c1"), Reactive]
        public string Categoria1 { get; set; }

        [JsonProperty (PropertyName = "c2"), Reactive]
        public string Categoria2 { get; set; }

        [JsonProperty (PropertyName = "c3"), Reactive]
        public string Categoria3 { get; set; }

        [JsonProperty (PropertyName = "c4"), Reactive]
        public string Categoria4 { get; set; }

        [JsonProperty (PropertyName = "chiave"), Reactive]
        public string Chiave { get; set; }

        }
    }

    
   