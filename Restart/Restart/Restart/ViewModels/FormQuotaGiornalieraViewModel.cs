using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Restart.Models;
using Restart.Services;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using Xamarin.Forms;

namespace Restart.ViewModels
    {
    class FormQuotaGiornalieraViewModel : ReactiveObject
        {

        [Reactive]
        public double TettoSpesa { get; set; } = 0;

        [Reactive]
        public double SommaIeri { get; set; }
        
        [Reactive]
        public double RimanenzaIeri { get; set; }
        [Reactive]
        public double RimanenzaOggi { get; set; }

        [Reactive]
        public Quota Quota { get; set; } = HomePageViewModel.Instance.Quota;

        public ReactiveCommand<bool,Unit> SalvaInserimento { get; }
        public Command Esci { get; }


        public static ICosmosDBService _cosmosService { get; set; }


         
            public FormQuotaGiornalieraViewModel (double TotaleOggi, double TotaleIeri)
            {

            _cosmosService = new CosmosDBService ("Quote");


            SalvaInserimento = ReactiveCommand.CreateFromTask<bool>(async (esci) =>
             {

                 Quota.TettoSpesa = TettoSpesa;

                 await _cosmosService.Salva (Quota, Quota.Tipo);

             

                 HomePageViewModel.Instance.Rimanenza = TettoSpesa - TotaleOggi;
                
                 HomePageViewModel.Instance.TettoSpesa = TettoSpesa;

                 if (esci)

                     await PopupNavigation.Instance.PopAsync (true);


             });


            Esci = new Command (async ( ) =>
            {
                await PopupNavigation.Instance.PopAsync (true);
            });



            if (( Quota.DataScelta - DateTime.Today ).Days == 0)


                TettoSpesa = Quota.TettoSpesa;

            else
                {

                SommaIeri = Quota.TettoSpesa;
                Quota.DataScelta = DateTime.Today;

                SalvaInserimento.Execute (false).Subscribe ( );

                
                }

            this.WhenAnyValue (x => x.TettoSpesa).Subscribe (valore => RimanenzaOggi = TettoSpesa - TotaleOggi);
            this.WhenAnyValue (x => x.SommaIeri).Subscribe (valore => RimanenzaIeri = valore - TotaleIeri);

            }

        }
    }
