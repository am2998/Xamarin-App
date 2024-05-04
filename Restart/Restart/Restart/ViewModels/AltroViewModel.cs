using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Restart.Models;
using Restart.Services;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Restart.ViewModels
    {
    class AltroViewModel : ReactiveObject
        {

      
        [Reactive]
        public  string Scelta { get; set; }

        public string Tipo { get; set; }
       

        [Reactive]
        public Categorie SlotCategorie { get; set; }


        public Command SalvaCategorie { get; set; }

        public Command SalvaQuota { get; set; }

        [Reactive]
        public string Categoria1 { get; set; }
        [Reactive]
        public string Categoria2 { get; set; }
        [Reactive]
        public string Categoria3 { get; set; }
        [Reactive]
        public string Categoria4 { get; set; }

        public ReactiveCommand<Unit, Unit> CaricaCategorie { get; }

        public static ReactiveCommand<Unit,AltroViewModel> Imposta { get; private set; }

        public ReactiveCommand<Unit, Unit> Esci { get; }

        private ICosmosDBService _cosmosService;


       

            public AltroViewModel (string tipo)
            {
            _cosmosService = new CosmosDBService ("Categorie");


            this.Tipo = tipo;

            CheckCategorie ( );

            this.WhenAnyValue (x => x.Scelta).Throttle(TimeSpan.FromSeconds(0.3)).Subscribe (s => MessageBus.Current.SendMessage<(string,string)> ((s = s != null ? s : Lingue.Resources.Categoria, Tipo)));

          

           

            Esci = ReactiveCommand.CreateFromTask (async ( ) =>
            {
                
                await PopupNavigation.Instance.PopAsync (true);
                });


            this.WhenAnyValue (x => x.Categoria1, x => x.Categoria2, x => x.Categoria3, x => x.Categoria4).Subscribe (lista => MessageBus.Current.SendMessage<(string,string,string,string)> (lista, "CategorieCambiate"));



            SalvaCategorie = new Command (async ( ) =>
            {
                SlotCategorie.Categoria1 = Categoria1;
                SlotCategorie.Categoria2 = Categoria2;
                SlotCategorie.Categoria3 = Categoria3;
                SlotCategorie.Categoria4 = Categoria4;

                await _cosmosService.Salva (SlotCategorie, SlotCategorie.Tipo);

                await PopupNavigation.Instance.PopAsync (true);
            });

   

            }


      

        async void CheckCategorie ( )
            {


            var list = await _cosmosService.OttieniCategorie("Aggiuntive", App._userID);

            if (list != null)
                {
                foreach (Categorie c in list)
                    if (c.Tipo.Equals ("Aggiuntive"))
                        SlotCategorie = c;
                }

            if (SlotCategorie == null)

                SlotCategorie = new Categorie { Tipo = "Aggiuntive", Chiave = App._userID };

            Categoria1 = SlotCategorie.Categoria1;
            Categoria2 = SlotCategorie.Categoria2;
            Categoria3 = SlotCategorie.Categoria3;
            Categoria4 = SlotCategorie.Categoria4;

            }

        }
    }
