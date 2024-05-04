
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Restart.Models;

using Restart.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;



using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Restart.Services;

namespace Restart.ViewModels
    {

    public sealed class CheckViewModel : ReactiveObject
        {
        public SourceList<Spesa> ListaSpese = new SourceList<Spesa> ( );

        public readonly IObservableCollection<Spesa> listaAccessibile = new ObservableCollectionExtended<Spesa> ( );

        public IObservableCollection<Spesa> ListaAccessibile => listaAccessibile;


     
   


        private SourceList<Risparmio> ListaRisparmi = new SourceList<Risparmio> ( );

        private readonly IObservableCollection<Risparmio> listaRisparmiAccessibile = new ObservableCollectionExtended<Risparmio> ( );

        public IObservableCollection<Risparmio> ListaRisparmiAccessibile => listaRisparmiAccessibile;



        public ReactiveCommand<Risparmio, Unit> Salva_Risparmio { get; private set; }

        public ReactiveCommand<Risparmio, Unit> Elimina_Risparmio { get; private set; }


        public ReactiveCommand<Spesa, Unit> Elimina_Spesa { get; private set; }


        public Command Salva_Spesa { get; private set; }



        public ReactiveCommand<string, Unit> CaricaListe { get; set; }


        [Reactive]
        public double TotaleSpese { get; set; }

        [Reactive]
        public double TotRisparmi { get; set; }


        private static readonly CheckViewModel instance = new CheckViewModel ( );


     
        private ICosmosDBService _cosmosServiceSpese;

        private ICosmosDBService _cosmosServiceRisparmi;


        static CheckViewModel ( )
            {
            }

        private CheckViewModel ( )
            {

            _cosmosServiceSpese = new CosmosDBService ("Spese");

            _cosmosServiceRisparmi = new CosmosDBService ("Risparmi");

            var myOperation = ListaSpese.Connect ( )

              .ObserveOn (RxApp.MainThreadScheduler)

                  .Bind (listaAccessibile)
                  .Subscribe (_ => TotaleSpese = ListaSpese.Items.Sum (x => x.Somma));
          



            ListaRisparmi.Connect ( )

          .Sort (SortExpressionComparer<Risparmio>.Descending (s => s.Somma))

          .ObserveOn (RxApp.MainThreadScheduler)
          .Bind (listaRisparmiAccessibile)
          .Subscribe (_ =>
          {

              foreach (Spesa s in ListaSpese.Items)
                  ControlloRisparmi (s);
              TotRisparmi = ListaRisparmi.Items.Sum (x => x.Somma);

          });


            CaricaListe = ReactiveCommand.CreateFromTask<string> (async (userid) =>
            {

                ListaSpese.Clear ( );

                ListaRisparmi.Clear ( );

                var temp = await _cosmosServiceSpese.OttieniSpese (userid);

                foreach (Spesa s in temp)
                    {
                    if (!s.Completa)

                        ListaSpese.Add (s);

                    }

                var result = await _cosmosServiceRisparmi.OttieniRisparmi (userid);

                foreach (Risparmio r in result)
                    ListaRisparmi.Add (r);
            });


            //////////////////////////////////////////////////////////////////////////////////


            CaricaListe.Execute ( ).Subscribe ( );




            //////////////////////////////////////////////////////////////////////////////////



            //////////////////////////////////////////////////////////////////////////////////



            Salva_Spesa = new Command<Spesa> (SalvaSpesa);

            Elimina_Spesa = ReactiveCommand.CreateFromTask<Spesa> (async (spesa) =>
            {

                EliminaRisparmiAssociati (spesa);

                ListaSpese.Remove (spesa);

                await _cosmosServiceSpese.Elimina (spesa.ID);

            });




            Salva_Risparmio = ReactiveCommand.CreateFromTask<Risparmio> (async r =>
{
    await _cosmosServiceRisparmi.Salva (r, r.ID);

    Spesa spesa = ListaSpese.Items.Where (s => s.ID == r.Spesa).FirstOrDefault ( );



    if (spesa != null)
        {

        spesa.TotaleRisparmi += r.Somma;

        await _cosmosServiceSpese.Salva (spesa, spesa.ID);
        ListaSpese.RemoveAt (ListaSpese.Items.IndexOf (spesa));
        ListaSpese.Add (spesa);


        }

    var indice = ListaRisparmi.Items.IndexOf (r);



    if (indice < 0)


        ListaRisparmi.Add (r);


    else


        ListaRisparmi.ReplaceAt (indice, r);

});







            Elimina_Risparmio = ReactiveCommand.CreateFromTask<Risparmio> (async r =>
            {
                

                                r = ListaRisparmi.Items.Where (risparmio => risparmio.ID.Equals(r.ID, StringComparison.OrdinalIgnoreCase)).FirstOrDefault ( );
            
                                if (r != null)
                                    {


                                    Spesa spesa = ListaSpese.Items.Where (s => s.ID == r.Spesa).FirstOrDefault ( );

                                    if (spesa != null)
                                   {
                        //await App.SQLiteDb.SaveItemAsync (spesa);

                        await _cosmosServiceSpese.Salva (spesa, spesa.ID);
                                        
                                        spesa.TotaleRisparmi -= r.Somma;

                                        }

                        

                                    ListaRisparmi.Remove (r);


                    await _cosmosServiceRisparmi.Elimina (r.ID);


                    }

                                    

            });


            //###########################  MESSAGGI ASCOLTATI


            MessageBus.Current.Listen<Spesa> ("NonCompleta").Subscribe (spesa => SalvaSpesa (spesa));
            MessageBus.Current.Listen<Risparmio> ( ).Subscribe (risparmio => Salva_Risparmio.Execute (risparmio).Subscribe ( ));


            MessageBus.Current.Listen<(int, Spesa)> ("SpesaModificata").Subscribe (param =>
            {
                if (!param.Item2.Completa)
                    {
                    Spesa s = ListaSpese.Items.Where (x => x.ID .Equals(param.Item1)).FirstOrDefault ( );

                    ListaSpese.Remove (s);

                    ListaSpese.Add (param.Item2);


                    }

            });



            //########################### FINE MESSAGGI ASCOLTATI
            }

        public static CheckViewModel Instance
            {
            get
                {
                return instance;
                }
            }



        public static Risparmio CreaRisparmiodaRimanenza (double rimanenza)
            {
            Risparmio r = new Risparmio
                {
                Somma = rimanenza,
                Nota = "...",
                Spesa = "-1",
                Chiave = App._userID

                };

            return r;
            }



        void EliminaRisparmiAssociati (Spesa s)
            {
            foreach (Risparmio r in ListaRisparmi.Items)
                {

                if (s.ID == r.Spesa)
                    Elimina_Risparmio.Execute (r).Subscribe ( );
                }
            }




        async void SalvaSpesa (Spesa s)
            {


            var indice = ListaSpese.Items.IndexOf (s);


            if (indice < 0)


                ListaSpese.Add (s);

            else


                ListaSpese.ReplaceAt (indice, s);


            // await App.SQLiteDb.SaveItemAsync (s);

            await _cosmosServiceSpese.Salva (s, s.ID);

            }


        void ControlloRisparmi (Spesa s)

            {

            s.TotaleRisparmi = 0;

            foreach (Risparmio r in ListaRisparmi.Items)
                {

                if (s.ID.Equals(r.Spesa))


                    s.TotaleRisparmi += r.Somma;

                }


            s.Differenza = s.Somma - s.TotaleRisparmi;


            }


        }

    }
    

            
