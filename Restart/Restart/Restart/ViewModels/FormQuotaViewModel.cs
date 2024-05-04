using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Restart.Models;
using Restart.Services;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace Restart.ViewModels
    {
    public class FormQuotaViewModel : ReactiveObject
        {

        [Reactive]
        public Quota Quota { get; set; }

        public ReactiveCommand<Unit,Unit> SalvaQuota { get; }
        public ReactiveCommand<Unit,Unit> EliminaQuota { get; }



        [Reactive]
        public DateTime DataMinima { get; set; }

        [Reactive]
        public string Progresso { get; set; }

        [Reactive]
        public double Rimanenza { get; set; }

        [Reactive]
        public double GiorniMancanti { get; set; }

        [Reactive]
        public double TettoSpesa { get; set; }

        [Reactive]
        public DateTime DataScelta { get; set; }

        [Reactive]
        public string Testo { get; set; } = null;

        [Reactive]
        public bool Mostra { get; set; } = false;

        public bool IsEnabled { get; set; } = false;

        public static ICosmosDBService _cosmosService { get; set; }
     


        public FormQuotaViewModel (string tipo, double TotaleOggi)
            {

            _cosmosService = new CosmosDBService ("Quote");

            Carica (tipo, TotaleOggi);

            EliminaQuota = ReactiveCommand.CreateFromTask (async() => {

                if(TettoSpesa!=TotaleOggi)

                TettoSpesa = TotaleOggi;

                Preferences.Set ("lock", TotaleOggi);

                Quota.TettoSpesa = TettoSpesa;

                await _cosmosService.Salva (Quota, Quota.Tipo);

                });


            this.WhenAnyValue (x => x.TettoSpesa).Subscribe (tetto =>

            {
             Rimanenza = tetto - TotaleOggi;

            if ((TotaleOggi / tetto ) * 100 > 90)
                Progresso = "#FF0000";

            else
                Progresso = "#ffffff"; 
            });

            var canSave = this.WhenAnyValue (x => x.TettoSpesa, tetto => tetto >= TotaleOggi);

            SalvaQuota = ReactiveCommand.CreateFromTask (async ( ) => {
            
                Quota.DataScelta = DataScelta;
                Quota.TettoSpesa = TettoSpesa;

               await _cosmosService.Salva (Quota, Quota.Tipo);


            },canSave);


        

          this.WhenAnyValue (x => x.DataScelta).Throttle(TimeSpan.FromSeconds(0.2)).Subscribe (async data =>
            {
           

                    GiorniMancanti = (DateTime.Today - data).Days;


                if (GiorniMancanti > 0)
                    {

                    Mostra = true;
                    Testo = Lingue.Resources.QuotaScaduta + " " + Rimanenza + " " + Lingue.Resources.VALUTA  + ", " + Lingue.Resources.MeseProssimo;
                    await Task.Delay (TimeSpan.FromSeconds (5));
                    Mostra = false;
         

                    if (Rimanenza != 0)
                        {
                        Risparmio r = CheckViewModel.CreaRisparmiodaRimanenza (Rimanenza);

                        MessageBus.Current.SendMessage<Risparmio> (r);

                        }

                    
                    if (Quota.Tipo.Equals ("Mensile"))
                        {

                        DataMinima = DataMinima.AddMonths (1);

                        DataScelta = data.AddMonths(1);

                        TettoSpesa = 0.00;


                        }

                    
                        EliminaQuota.Execute().Subscribe();
                    

                    }

            });

          
 
            }


        async void GestisciRimanenza ( )
            {
            Mostra = true;
            Testo = Lingue.Resources.QuotaRaggiunta;
            await Task.Delay (TimeSpan.FromSeconds (5));
            Mostra = false;


            EliminaQuota.Execute ().Subscribe();

            }


        public async void Carica (string tipo, double TotaleOggi)
            {


            if (tipo.Equals ("A scelta"))


                DataMinima = DateTime.Today.AddDays (1);


            else

                DataMinima = DateTime.Today.AddMonths (1);

            List<Quota> lista = await _cosmosService.OttieniQuota (tipo, App._userID);

            if (lista != null && lista.Count!=0)

                Quota = lista.FirstOrDefault();

                    

            if (Quota == null)
                {

                Preferences.Set ("lock", TotaleOggi);

                Quota = new Quota {Tipo = tipo, DataScelta = DataMinima, TettoSpesa = Preferences.Get ("lock", TotaleOggi), Chiave = App._userID };

                }


            TettoSpesa = Quota.TettoSpesa;

            DataScelta = Quota.DataScelta;


            if (TettoSpesa != Preferences.Get ("lock", TotaleOggi))
                {

                Rimanenza = TettoSpesa - TotaleOggi;


                if (Rimanenza < 0)

                    GestisciRimanenza ( );

                else if (( TotaleOggi / TettoSpesa ) * 100 > 90)
                    Progresso = "#FF0000";

                else
                    Progresso = "#ffffff";

                }

            else
                EliminaQuota.Execute ( ).Subscribe ( );


            }

        }
    }
    
