using ReactiveUI.Fody.Helpers;
using Restart.Models;
using Restart.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Restart.Views
    {
    [XamlCompilation (XamlCompilationOptions.Compile)]
    public partial class FormRisparmio : Rg.Plugins.Popup.Pages.PopupPage
        {


        FormRisparmioViewModel vm;

        public FormRisparmio (Risparmio _oggetto)
            {
            vm = new FormRisparmioViewModel (_oggetto);

            this.BindingContext = vm;

            InitializeComponent ( );
    
     
            }


        async void Annulla (object sender, EventArgs e)
            {
            await PopupNavigation.Instance.PopAsync (true);
            }

         

        void Salva (object sender, EventArgs e)
            {
            CheckViewModel.Instance.Salva_Risparmio.Execute (vm.Risparmio).Subscribe (async _ => await PopupNavigation.Instance.PopAsync (true));


            }

        void Elimina (object sender, EventArgs e)
            {
           
            CheckViewModel.Instance.Elimina_Risparmio.Execute (vm.Risparmio).Subscribe (async _ => await PopupNavigation.Instance.PopAsync (true));

            }


        async void Assegna (object sender, EventArgs e)
            {
            await PopupNavigation.Instance.PushAsync (new ListaSpeseNonComplete (vm.Risparmio));
            }
       
        }
    }