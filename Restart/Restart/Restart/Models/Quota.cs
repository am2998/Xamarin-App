using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace Restart.Models
{


    
    public class Quota : ReactiveObject
       
    {
        [JsonProperty (PropertyName = "id"), Reactive]
        public string Tipo { get; set; }


        [JsonProperty (PropertyName = "tettoSpesa"), Reactive]
        public double TettoSpesa { get; set; }

        [JsonProperty (PropertyName = "dataScelta"), Reactive]
        public DateTime DataScelta { get; set; }
        
        [JsonProperty (PropertyName = "chiave"), Reactive]
        public string Chiave { get; set; }

     
        }
}
