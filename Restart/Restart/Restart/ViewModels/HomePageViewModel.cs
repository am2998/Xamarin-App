using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;




using Restart.Models;
using Xamarin.Forms;

using DynamicData;
using ReactiveUI;
using DynamicData.Binding;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;
using System.Reactive;
using Xamarin.CommunityToolkit.Helpers;
using Rg.Plugins.Popup.Services;
using Restart.Views;
using System.Threading.Tasks;
using Restart.Services;
using Plugin.LocalNotifications;
using Xamarin.Essentials;

namespace Restart.ViewModels
    {
    public class HomePageViewModel : ReactiveObject
        {



        ImpostazioniViewModel vm = new ImpostazioniViewModel ();

        public Command Elimina { get; set; }
        public ReactiveCommand<Spesa,Unit> SalvaSpesa { get; private set; }

  

        private SourceList<Spesa> Spese_Sostenute { get; set; } = new SourceList<Spesa> ( );

        private readonly IObservableCollection<Spesa> listaSpeseAccessibile = new ObservableCollectionExtended<Spesa> ( );
        public IObservableCollection<Spesa> ListaSpeseAccessibile => listaSpeseAccessibile;

  
   


        [Reactive]
        public double TotaleSpese { get; set; }

        [Reactive]
        public double TettoSpesa { get; set; } 

        [Reactive]
        public double Rimanenza { get; set; }

        [Reactive]
        public string CategoriaSelezionata { get; set; }


        [Reactive]
        public double TotaleRisparmiQuote { get; set; }

        [Reactive]
        public Quota Quota { get; set; }

        [Reactive]
        public double Somma { get; set; }

        [Reactive]
        public string Categoria { get; set; }



        [Reactive]
        public Boolean DisplayL { get; set; }

        [Reactive]
        public Boolean DisplayM { get; set; }



        [Reactive]
        public DateTime DataMinima { get; set; }

        [Reactive]
        public double QuantitaOggi { get; set; }

        [Reactive]
        public double QuantitaIeri { get; set; }


        public ReactiveCommand<Unit, Unit> ApriCategorie {get;}

        public ReactiveCommand<Unit, Unit> ApriQuotaGiornaliera { get; }


        public LocalizedString AppVersion { get; }

       private static readonly HomePageViewModel instance = new HomePageViewModel ( );


        public static HomePageViewModel Instance
            {
            get
                {
                return instance;
                }
            }

        public static ICosmosDBService _cosmosService { get; set; }



            static HomePageViewModel ( )
                {

                }

        private HomePageViewModel ( )
            {

            _cosmosService = new CosmosDBService ("Quote");


            ApriQuotaGiornaliera = ReactiveCommand.CreateFromTask (async ( ) =>
             {
                 List<Quota> lista = await _cosmosService.OttieniQuota ("Giornaliera", App._userID);


                 if (lista != null && lista.Count!=0)

                     Quota = lista.FirstOrDefault ( );




                  if (Quota == null)

                     Quota = new Quota { Tipo = "Giornaliera", TettoSpesa = 0.00, DataScelta = DateTime.Today.AddDays(1), Chiave = App._userID };


                 var SpeseOggi = Spese_Sostenute.Items.Where (x => ( x.Data.Day - DateTime.Today.Day ) == 0).Sum (x => x.Somma);
                 var SpeseIeri = Spese_Sostenute.Items.Where (x => ( x.Data.Day - DateTime.Today.Day ) == -1).Sum (x => x.Somma);

                 if(DateTime.Compare(Quota.DataScelta,DateTime.Today)!=0)

                 await PopupNavigation.Instance.PushAsync (new FormQuotaGiornaliera (SpeseOggi, SpeseIeri));
             });



            DisplayL = true;
            DisplayM = false;



            this.WhenAnyValue (x => x.vm.Lingua).Throttle (TimeSpan.FromSeconds (0.2)).Subscribe (_ => CategoriaSelezionata = Lingue.Resources.Categoria);


            ApriCategorie = ReactiveCommand.CreateFromTask (async ( ) => await PopupNavigation.Instance.PushAsync (new Views.Categorie ("Inserimento")));



            Elimina = new Command<Spesa> (Elimina_Spesa_Dal_Database);

            Spese_Sostenute.Connect ( )
        .Sort (SortExpressionComparer<Spesa>.Descending (s => s.Data))
        .ObserveOn (RxApp.MainThreadScheduler)
        .Bind (listaSpeseAccessibile)
        .Subscribe (_ =>
        {
            TotaleSpese = Spese_Sostenute.Items.Where (x => x.Data.Month == DateTime.Today.Month).Sum (x => x.Somma);

            QuantitaOggi = Spese_Sostenute.Items.Where (x => ( x.Data.Day - DateTime.Today.Day ) == 0).Sum (x => x.Somma);
            QuantitaIeri = Spese_Sostenute.Items.Where (x => ( x.Data.Day - DateTime.Today.Day ) == -1).Sum (x => x.Somma);

           Rimanenza = TettoSpesa - QuantitaOggi;

            if (Spese_Sostenute.Count == 0)
                {
                DisplayL = false;
                DisplayM = true;
                }
            else
                {
                DisplayL = true;
                DisplayM = false;
                }

        });



            SalvaSpesa = ReactiveCommand.CreateFromTask<Spesa> (async s =>
             {

                 MessageBus.Current.SendMessage<Spesa> (s, "SpesaSalvata");


                 Spesa spesa = Spese_Sostenute.Items.Where (x => x.ID.Equals (s.ID)).FirstOrDefault ( );


                     if (spesa==null)


                         Spese_Sostenute.Add (s);

                     else

                         Spese_Sostenute.Replace (spesa, s);



                     await App._cosmosServiceSpese.Salva (s, s.ID);
                     

             });


            //###########################  MESSAGGI ASCOLTATI


            MessageBus.Current.Listen<(bool, IEnumerable<Spesa>)> ("ListaPartenzaSpese").Subscribe (param =>
            {

                Spese_Sostenute.Clear ( );
                Spese_Sostenute.AddRange (param.Item2);

                if (param.Item1)
                    {
                    DisplayL = false;
                    DisplayM = true;
                    }

            });

            MessageBus.Current.Listen<string> ("CambiaLingua").Throttle (TimeSpan.FromSeconds (0.1)).Subscribe (_ => CategoriaSelezionata = Lingue.Resources.Categoria);



            MessageBus.Current.Listen<(string, string)> ( ).Subscribe (param => CategoriaSelezionata = param.Item2.Equals ("Inserimento") ? param.Item1 : CategoriaSelezionata);


            MessageBus.Current.Listen<(int, Spesa)> ("SpesaEliminata").Subscribe (param => { Spesa s = Spese_Sostenute.Items.Where (x => x.ID.Equals(param.Item1)).FirstOrDefault ( ); Spese_Sostenute.Remove (s); });



            MessageBus.Current.Listen<(string, Spesa)> ("SpesaModificata").Subscribe (param =>
             {

                 Spesa s = Spese_Sostenute.Items.Where (x => x.ID.Equals(param.Item1)).FirstOrDefault ( );


                 Spese_Sostenute.ReplaceAt (Spese_Sostenute.Items.IndexOf (s), param.Item2);
             }
            );



            MessageBus.Current.Listen<Spesa> ("SpesaNonCompletaOraCompletata").Subscribe (spesa => Spese_Sostenute.Add (spesa));

            }



        //########################### FINE  MESSAGGI ASCOLTATI

        async void Elimina_Spesa_Dal_Database (Spesa s)
            {

            MessageBus.Current.SendMessage<(string, Spesa)> ((s.ID, s), "EliminaSpesa");


            Spese_Sostenute.Remove (s);


            await App._cosmosServiceSpese.Elimina (s.ID);

            }


    


        }
    }
