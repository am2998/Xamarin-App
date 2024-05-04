using Restart.Models;
using System.Linq;
using Xamarin.Forms;
using ReactiveUI;
using DynamicData;
using DynamicData.Binding;
using System;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;
using Microcharts;
using SkiaSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.Reactive;
using Rg.Plugins.Popup.Services;
using Restart.Services;

namespace Restart.ViewModels
    {


    public sealed class SpeseViewModel : ReactiveObject
        {

        

        #region Lista + Comandi

        public SourceList<Spesa> ListaSpese { get; set; } = new SourceList<Spesa> ();

        private readonly IObservableCollection<Spesa> listaOsservabile = new ObservableCollectionExtended<Spesa> ( );
        public IObservableCollection<Spesa> ListaOsservabile => listaOsservabile;




        private string _scelta = Lingue.Resources.Categoria;

        public string Scelta
            {
            get { return Preferences.Get ("Scelta", _scelta); }
            set
                {
                Preferences.Set ("Scelta", value);
               
                this.RaiseAndSetIfChanged (ref _scelta, value);
                
                }
            }


        private string _ordine = Lingue.Resources.DATA;

        public string Ordine
            {
            get { return Preferences.Get ("Ordine", _ordine); }
            set
                {
                Preferences.Set ("Ordine", value);
                this.RaiseAndSetIfChanged (ref _ordine, value);
                }
            }

        private double _somma = 2000;

        public double Somma
            {
            get { return Preferences.Get ("Somma", _somma); }
            set
                {
                Preferences.Set ("Somma", value);
                this.RaiseAndSetIfChanged (ref _somma, value);
                }
            }




        public Command CaricaGrafico { get; private set; }

        public Command Elimina { get; private set; }

        public Command Salva { get; private set; }

        public ReactiveCommand<Unit,Unit> Aggiorna { get; private set; }

 


        public ReactiveCommand<Unit,Unit> ApriCategorie { get; private set; }


        #endregion


        #region Proprieta'


        [Reactive]
        public Chart Chart { get; set; }


        [Reactive]
        public Boolean Refresh { get; set; }



        [Reactive]
        public ChartEntry[] entries { get; set; }

       
        public Categorie Categorie;

        public List<Spesa> _listaAggiuntiva { get; set; } = new List<Spesa> ( );

        public ReactiveCommand <string,Unit> CaricaSpese { get; private set; }



        private static readonly SpeseViewModel instance = new SpeseViewModel ( );

        public static SpeseViewModel Instance
            {
            get
                {
                return instance;
                }
            }


        #endregion

        #region Costruttore

        public ICosmosDBService _cosmosService { get; set; }

        static SpeseViewModel ( )
            {
            }

        private SpeseViewModel ( )
            {
            _cosmosService = new CosmosDBService ("Categorie");

            CaricaSpese = ReactiveCommand.CreateFromTask<string> ( async (userid) =>
            {

                    var temp = await App._cosmosServiceSpese.OttieniSpese(userid);

                    var filtered = temp.Where (x => x.Completa);

                    if (temp != null && temp.Count != 0)

                        ListaSpese.Clear ( );

                    ListaSpese.AddRange (filtered);

                    _listaAggiuntiva.Clear ();

                    _listaAggiuntiva.AddRange (filtered);

                });

            CaricaSpese.Execute (App._userID).Subscribe ( );

          /*  MessageBus.Current.Listen<(bool, IEnumerable<Spesa>)> ("ListaPartenzaSpese").Subscribe (param =>
            {

                ListaSpese.Clear ( );
                ListaSpese.AddRange (param.Item2);

                _listaAggiuntiva.Clear ( );

                _listaAggiuntiva.AddRange (param.Item2);

            });

            */


                ApriCategorie = ReactiveCommand.CreateFromTask (async ( ) => await PopupNavigation.Instance.PushAsync (new Views.Categorie ("Filtraggio")));

         

            IObservable<SortExpressionComparer<Spesa>> ComparaOsservabile = this.WhenAnyValue (x => x.Ordine).Select (x =>
              {
                  if (x.Equals(Lingue.Resources.DATA))
                      return SortExpressionComparer<Spesa>.Descending (s => s.Data);
                  else
                      return SortExpressionComparer<Spesa>.Descending (s => s.Somma);
              });
          
  

            this.WhenAnyValue (x => x.Somma, x => x.Scelta).Subscribe (param =>
            {
                var lista = _listaAggiuntiva;

                var spese = lista.Where(x => x.Completa);


                if (param.Item2.Equals (Lingue.Resources.Categoria))

                    spese = spese.Where (x => x.Somma <= param.Item1);

                else
                    spese = spese.Where (x => x.Somma <= param.Item1 && x.Categoria.Equals (param.Item2));



                ListaSpese.Clear ( );

                ListaSpese.AddRange (spese);
            });



            Elimina = new Command<Spesa> (Elimina_Spesa_Dal_Database);




            Aggiorna = ReactiveCommand.Create ( ( ) =>
             {

                 Refresh = true;

                 ListaSpese.Clear ( );
                 

                 if (!Scelta.Equals (Lingue.Resources.Categoria))
                     ListaSpese.AddRange (_listaAggiuntiva.Where (x => x.Completa && x.Somma <= Somma && x.Categoria.Equals (Scelta)));

                 else
                     ListaSpese.AddRange (_listaAggiuntiva.Where (x => x.Completa && x.Somma <= Somma));



                 Refresh = false;

             });


            CaricaGrafico = new Command (_CaricaGrafico);





            ListaSpese.Connect ( )
            .Sort (ComparaOsservabile)
             .ObserveOn (RxApp.MainThreadScheduler)
             .Bind (listaOsservabile)

             .Subscribe (_ => CaricaGrafico.Execute (null));


            //###########################  MESSAGGI ASCOLTATI

            MessageBus.Current.Listen<(string, string)> ( ).Subscribe (param => Scelta = param.Item2.Equals ("Filtraggio") ? param.Item1 : Scelta);



            MessageBus.Current.Listen<(string,Spesa)> ("SpesaModificata").Subscribe (param =>

            {

                   Spesa s = ListaSpese.Items.Where (x => x.ID.Equals(param.Item1)).FirstOrDefault ( );


                if (s != null)
                    {
                    ListaSpese.Replace (s, param.Item2);

                    _listaAggiuntiva.Replace (s, param.Item2);
                    }


            });

            MessageBus.Current.Listen<(string, string, string, string)> ("CategorieCambiate").Subscribe(_ => CaricaGrafico.Execute(null));

            MessageBus.Current.Listen<Spesa> ("SpesaSalvata").Subscribe (param => { ListaSpese.Add (param); _listaAggiuntiva.Add (param); });
            


            MessageBus.Current.Listen<(string,Spesa)> ("EliminaSpesa").Subscribe (param =>
            {
                Spesa s = ListaSpese.Items.Where (x => x.ID.Equals(param.Item1)).FirstOrDefault ( );

                ListaSpese.Remove (s);

                _listaAggiuntiva.Remove (s);

            });


            MessageBus.Current.Listen<Spesa> ("SpesaNonCompletaOraCompletata").Subscribe (spesa => { ListaSpese.Add (spesa); _listaAggiuntiva.Add (spesa); });
       

            //########################### FINE MESSAGGI ASCOLTATI



            }


        #endregion

        #region Methods



         async public void _CaricaGrafico ( )
            {


            List<Categorie> lista = await _cosmosService.OttieniCategorie ("Aggiuntive", App._userID);

            if (lista != null && lista.Count!=0)

                Categorie = lista.FirstOrDefault ();


            if (Categorie == null)
                Categorie = new Categorie { Tipo = "Aggiuntive" };

            if (Categorie.Categoria1==null)
                Categorie.Categoria1 = "//";
           
            if (Categorie.Categoria2 == null)
                Categorie.Categoria2 = "//";
            
            if (Categorie.Categoria3 == null)
                Categorie.Categoria3 = "//";
            
            if (Categorie.Categoria4 == null)
                Categorie.Categoria4 = "//";


            float Cibo = 0;
            float Trasporti = 0;
           float  Cultura = 0;
            float Istruzione = 0;
            float Divertimenti = 0;
            float Regali = 0;
            float Altro = 0;
            float Bellezza = 0;
            float Salute = 0;
            float Abbigliamento = 0;

            float Categoria1 = 0;
            float Categoria2 = 0;
            float Categoria3 = 0;
            float Categoria4 = 0;


            foreach (Spesa s in ListaSpese.Items)
                {
                if (s.Categoria != null)
                    {

                    if (s.Categoria.Equals (Lingue.Resources.Cibo))
                        Cibo += (float)s.Somma;

                    else if (s.Categoria.Equals (Lingue.Resources.Trasporti))
                        Trasporti += (float)s.Somma;

                    else if (s.Categoria.Equals (Lingue.Resources.Cultura))
                        Cultura += (float)s.Somma;

                    else if (s.Categoria.Equals (Lingue.Resources.Bellezza))
                        Bellezza += (float)s.Somma;

                    else if (s.Categoria.Equals (Lingue.Resources.Istruzione))
                        Istruzione += (float)s.Somma;

                    else if (s.Categoria.Equals (Lingue.Resources.Regali))
                        Regali += (float)s.Somma;

                    else if (s.Categoria.Equals (Lingue.Resources.Divertimenti))

                        Divertimenti += (float)s.Somma;

                    else if (s.Categoria.Equals (Lingue.Resources.Salute))

                        Salute += (float)s.Somma;

                    else if (s.Categoria.Equals (Lingue.Resources.Abbigliamento))

                        Abbigliamento += (float)s.Somma;



                    else if (String.Equals (s.Categoria, Categorie.Categoria1))
                        Categoria1 += (float)s.Somma;


                    else if (String.Equals (s.Categoria, Categorie.Categoria2))
                        Categoria2 += (float)s.Somma;



                    else if (String.Equals (s.Categoria, Categorie.Categoria3))
                        Categoria3 += (float)s.Somma;


                    else if (String.Equals (s.Categoria, Categorie.Categoria4))
                         Categoria4 += (float)s.Somma;
                      

                    else

                        Altro += (float)s.Somma;
                    }
                }


                    
                

          


  entries = new[] {
                    CreaEntry (Cibo, Lingue.Resources.Cibo, "#fffdd0"),

                   CreaEntry(Trasporti, Lingue.Resources.Trasporti, "#e6e4bb"),

                   CreaEntry (Cultura, Lingue.Resources.Cultura, "#f3b6b6"),

               CreaEntry (Bellezza, Lingue.Resources.Bellezza, "#ffd7d7"),

               CreaEntry (Regali, Lingue.Resources.Regali, "#32CD32"),

               CreaEntry (Istruzione, Lingue.Resources.Istruzione, "#00CD00"),

               CreaEntry (Divertimenti, Lingue.Resources.Divertimenti, "#006400"),
               
               CreaEntry (Salute, Lingue.Resources.Salute, "#FFFFFF"),

               CreaEntry (Altro, Lingue.Resources.Altro, "#009900"),

               CreaEntry (Abbigliamento, Lingue.Resources.Abbigliamento, "#008800"),

                CreaEntry (Categoria1, Categorie.Categoria1, "#735895"),

                 CreaEntry (Categoria2, Categorie.Categoria2, "#b15394"),

                  CreaEntry (Categoria3, Categorie.Categoria3, "#e54e77"),

                   CreaEntry (Categoria4, Categorie.Categoria4, "#ff6347")


                };

            this.Chart = new DonutChart ( )
                {
                Entries = entries,
                LabelTextSize = 25,
                IsAnimated = true,
                LabelMode = LabelMode.RightOnly,

                BackgroundColor = SKColor.Parse ("#121212")
                };

            }




      /*   void Salva_Spesa (Spesa s)
            {

            Spesa spesa = ListaSpese.Items.Where (x => x.ID.Equals(s.ID)).FirstOrDefault();

           
            
            ListaSpese.Replace (spesa, s);

            _listaAggiuntiva.Replace (spesa, s);
            }

        */

        async void Elimina_Spesa_Dal_Database (Spesa s)
            {

            ListaSpese.Remove (s);

            _listaAggiuntiva.Remove (s);

            MessageBus.Current.SendMessage<(string, Spesa)> ((s.ID, s), "SpesaEliminata");


            await App._cosmosServiceSpese.Elimina (s.ID);



            }

        ChartEntry CreaEntry (float value, string Label, string Color)
            {
            return new ChartEntry (value)
                {
                Label = Label,
                ValueLabel = value.ToString ( ),
                Color = SKColor.Parse (Color),
                ValueLabelColor = SKColor.Parse (Color),
                TextColor = SKColor.Parse (Color),

                };

            }

        #endregion

        }
        
}
   







