using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restart.ViewModels
    { 
    public class IntroViewModel : ReactiveObject
    { 
    

  
        public List<Object> Lista { get; set; }

        public IntroViewModel ( )
            {
            var prima = new { Immagine = "accounting.png", Testo = Lingue.Resources.RegistraSpese };
            var seconda = new { Immagine = "clipboard.png", Testo = Lingue.Resources.GestisciQuota };
            var terza = new { Immagine = "piggy.png", Testo = Lingue.Resources.GestisciRisparmi };
            Lista = new List<object> { prima, seconda, terza };

            }
        }
    }
