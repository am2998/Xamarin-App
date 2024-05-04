using ReactiveUI;
using Restart.Models;
using Restart.ViewModels;

using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Restart.Views
{
    [XamlCompilation (XamlCompilationOptions.Compile)]
    public partial class CheckPage : ContentPage
        {
     

        public CheckPage ( )
            {
            InitializeComponent ( );
            this.BindingContext = CheckViewModel.Instance;

            }


        async void ModellaRisparmio (object sender, EventArgs e)
            {

            await PopupNavigation.Instance.PushAsync (new FormRisparmio (null));

            }


        protected async override void OnAppearing ( )
            {
            base.OnAppearing ( );


            var current = Connectivity.NetworkAccess;

           if(current != NetworkAccess.Internet)
            
           await PopupNavigation.Instance.PushAsync (new PopUpGenerico (Lingue.Resources.Errore, Lingue.Resources.NoConnessione, "categoria3.png", "Connessione"));
       
            }




            //#########################################################################################################################################################################

            async void Gestisci_Spesa (object sender, EventArgs e)
                {

                Plugin.InputKit.Shared.Controls.CheckBox cb = sender as Plugin.InputKit.Shared.Controls.CheckBox;

                Spesa spesa = cb.BindingContext as Spesa;



                if (cb.IsChecked)
                    {

                    await PopupNavigation.Instance.PushAsync (new FormSpesa (spesa));

                    cb.IsChecked = false;
                    }

                }








            async void Gestisci_Risparmio (object sender, EventArgs e)
                {

                Plugin.InputKit.Shared.Controls.CheckBox cb = sender as Plugin.InputKit.Shared.Controls.CheckBox;



                Risparmio r = cb.BindingContext as Risparmio;


                if (r != null && cb.IsChecked == true)

                    await PopupNavigation.Instance.PushAsync (new FormRisparmio (r));

                }
            }
        }
        
 
