using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Restart.Models;
using Restart.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace Restart.ViewModels
    {
    class FormRisparmioViewModel : ReactiveObject
        {
      


        [Reactive]
        public Risparmio Risparmio { get; set; }

        [Reactive]
        public Spesa Spesa { get; set; }

      

        public ReactiveCommand<Unit,Unit> SalvaRisparmio { get; }

        public ReactiveCommand<Unit, Unit> Elimina { get; }

        public ReactiveCommand<Risparmio, Unit> Assegna { get; }



        public FormRisparmioViewModel (Risparmio Oggetto)
            {
            if (Oggetto is null)


                Risparmio = new Risparmio { ID = Guid.NewGuid().ToString(), Somma = 0.00, Nota = "//", Spesa = "-1", Chiave = App._userID };

            else
                {

                Risparmio = Oggetto;

                Spesa = CheckViewModel.Instance.ListaSpese.Items.Where (x => x.ID.Equals(Risparmio.Spesa)).FirstOrDefault ( );

                }
            MessageBus.Current.Listen<Spesa> ("SpesaAssegnata").Subscribe (s => Spesa = s);

            }
        }
    }
