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
    public partial class FormQuota : Rg.Plugins.Popup.Pages.PopupPage
        {

        FormQuotaViewModel vm;


        public FormQuota (string tipo, double TotaleSpese)
            {
            InitializeComponent ( );

            vm = new FormQuotaViewModel (tipo,TotaleSpese);

            BindingContext = vm;

            if (tipo.Equals ("Mensile"))

                Titolo.Text = Lingue.Resources.QuotaMensile;

            else
                Titolo.Text = Lingue.Resources.QuotaLibera;


            }


        async void TornaIndietro(object sender, EventArgs e)
            {
            await PopupNavigation.Instance.PopAsync (true);
            }


        protected override void OnAppearing ( )
            {
            base.OnAppearing ( );
      

            }





        }
    }