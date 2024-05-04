using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Restart.Models;
using Restart.Services;
using Restart.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using Xamarin.Forms;


namespace Restart.ViewModels
    {

    class FormSpesaViewModel : ReactiveObject
        {


        [Reactive]
        public Spesa Spesa { get; set; }


        [Reactive]
        public string Scelta { get; set; }


        [Reactive]
        public bool TipoSpesa { get; set; }

        [Reactive]
        public ReactiveCommand<Unit,Unit> Salva {get; private set;}

        public ReactiveCommand<Unit,Unit> ApriCategorie { get; private set; }



            public FormSpesaViewModel (Spesa s)


            {
            Spesa = s;

            Scelta = Spesa.Categoria;

            TipoSpesa = s.Completa;

            Salva = ReactiveCommand.CreateFromTask (async () =>
            {

                Spesa.Differenza = Spesa.Somma - Spesa.TotaleRisparmi;
                await App._cosmosServiceSpese.Salva (Spesa, Spesa.ID);

                MessageBus.Current.SendMessage<(string, Spesa)> ((Spesa.ID, Spesa), "SpesaModificata");

                await PopupNavigation.Instance.PopAsync (true);
            });



            ApriCategorie = ReactiveCommand.CreateFromTask (async ( ) => await PopupNavigation.Instance.PushAsync (new Views.Categorie ("Modifica")));

            MessageBus.Current.Listen<(string, string)> ( ).Subscribe (param =>
            {

                Scelta = param.Item2.Equals ("Modifica") ? param.Item1 : Scelta;

                Spesa.Categoria = Scelta;

            });




            }

        }
    }
    
