
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Restart.Models;
using Restart.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Restart.Views
    {
    [XamlCompilation (XamlCompilationOptions.Compile)]
    public partial class FormSpesa : Rg.Plugins.Popup.Pages.PopupPage
        {

        FormSpesaViewModel vm;
         

        public FormSpesa (Spesa s) 
            {

            InitializeComponent ( );

            vm = new FormSpesaViewModel (s);

            this.BindingContext = vm;

            Elimina.BindingContext = CheckViewModel.Instance;

            Elimina.CommandParameter = s;


            }


        async void Annulla(object sender, EventArgs e)
            {
            await PopupNavigation.Instance.PopAsync (true);
            }

        async void EliminaSpesa (object sender, EventArgs e)
            {
            CheckViewModel.Instance.Elimina_Spesa.Execute (vm.Spesa).Subscribe ( );
            await PopupNavigation.Instance.PopAsync (true);
            }

        async void AssegnaSpesa (object sender, EventArgs e)
            {
            vm.Spesa.Completa = true;

            MessageBus.Current.SendMessage<Spesa> (vm.Spesa, "SpesaNonCompletaOraCompletata");

            CheckViewModel.Instance.Elimina_Spesa.Execute (vm.Spesa).Subscribe ( );

            await PopupNavigation.Instance.PopAsync (true);
            }

        }
    }