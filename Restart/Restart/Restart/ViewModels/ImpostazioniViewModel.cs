using DynamicData;
using DynamicData.Binding;
using Plugin.LocalNotifications;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Restart.Models;
using Restart.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Restart.ViewModels
    {
    public class ImpostazioniViewModel : ReactiveObject
        {




        [Reactive]
        public TimeSpan Orario { get; set; } = Preferences.Get ("Orario", DateTime.Today.AddHours (18)) - DateTime.Today;

        [Reactive]
        public bool Attivazione { get; set; } = Preferences.Get ("Localizzazione", false);

   

        [Reactive]
        public bool Sblocco { get; set; } = Preferences.Get ("Fingerprint", false);

  
        public string lingua = Preferences.Get ("Lingua", "Italiano");
           


        public string Lingua
            {
            get
                {
               
                ImpostaLingua (lingua);
                return lingua;

                }
            set
                {
                if (Lingua == value)
                    return;

                Preferences.Set ("Lingua", value);
                this.RaiseAndSetIfChanged (ref lingua, value);

                ImpostaLingua (Lingua);
             
                MessageBus.Current.SendMessage<string> (null, "CambiaLingua");
                }
            }

        

        public ReactiveCommand<Unit,Unit> SalvaImpostazioni { get; private set; }
   

        public ReactiveCommand <Unit,Unit> OttieniLingue { get; }

        public ImpostazioniViewModel ( )
            {


            SalvaImpostazioni = ReactiveCommand.CreateFromTask (async ( ) =>
              {
                  

                  var OrarioSalvato = Preferences.Get ("Orario", DateTime.Today.AddHours(18)) - DateTime.Today;

                  if (TimeSpan.Compare (OrarioSalvato, Orario) != 0)
                      {
                      Preferences.Set ("Orario", DateTime.Today + Orario);

                      CrossLocalNotifications.Current.Cancel (0);
                      CrossLocalNotifications.Current.Show (Lingue.Resources.Hey, Lingue.Resources.Ricordati, 0, DateTime.Today + Orario);
                      

                      }


                  var ImpostazioniMappa = Preferences.Get ("Localizzazione", false);

                 if(ImpostazioniMappa.Equals(Attivazione)==false)
                      Preferences.Set ("Localizzazione", Attivazione);


                  var SbloccoImpronta = Preferences.Get ("Fingerprint", false);

                  if (Sblocco.Equals (SbloccoImpronta) == false)
                      Preferences.Set ("Fingerprint", Sblocco);



                    await Shell.Current.GoToAsync ("..");


              });
        


            }


        void ImpostaLingua (string Lingua)
            {
            string temp;
            if (Lingua.Equals ("English"))
                temp = "en-US";
            else if (lingua.Equals ("Español"))
                temp = "es-ES";
            else if (lingua.Equals ("French"))
                temp = "FR";
            else if (lingua.Equals ("日本語"))
                temp = "JA";
            else if (lingua.Equals ("Deutsch"))
                temp = "DE";
            else if (lingua.Equals ("русский"))
                temp = "RU";
            else if (lingua.Equals ("中文"))
                temp = "ZH";

            else
                temp = "it-IT";
            
            LocalizationResourceManager.Current.SetCulture (CultureInfo.GetCultureInfo (temp));
            }

      
        }
    }

      