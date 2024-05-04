using Microcharts;
using Restart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Restart.Lingue;
using SkiaSharp;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Restart.ViewModels;
using ReactiveUI.XamForms;
using ReactiveUI;
using Restart.Views;
using Xamarin.Forms.Maps;

namespace Restart.Views
    {
    [XamlCompilation (XamlCompilationOptions.Compile)]


    public partial class SpeseFattePage : ReactiveContentPage<SpeseViewModel>
        {


        public SpeseFattePage ( )
            {

            InitializeComponent ( );

            BindingContext = SpeseViewModel.Instance;


            }


        //######################################################################################################################################




        public async void ApriFiltro (object sender, EventArgs e)
            {

            await PopupNavigation.Instance.PushAsync (new Filtro());
            }



    //###########################################################################################################################################################
   


            async void MostraNota(object sender, EventArgs e)
            {
                ImageButton b = sender as ImageButton;
                Spesa s = b.BindingContext as Spesa;

                if (s != null) await DisplayAlert("", s.Nota, "OK");

            }




        //###########################################################################################################################################################



        async void ApriStatistiche(object sender, EventArgs e)
            {
            await PopupNavigation.Instance.PushAsync (new Statistiche ( ));
            }



        void Elimina_Spesa (object sender, EventArgs e)
            {

            ImageButton ib = sender as ImageButton;

            Spesa s = ib.BindingContext as Spesa;

            SpeseViewModel.Instance.Elimina.Execute (s);

            }


        protected async override void OnAppearing ( )
            {
            base.OnAppearing ( );


            var current = Connectivity.NetworkAccess;

            if (current != NetworkAccess.Internet)

                await PopupNavigation.Instance.PushAsync (new PopUpGenerico (Lingue.Resources.Errore, Lingue.Resources.NoConnessione, "categoria3.png", "Connessione"));

            }

            async void ApriMappa (object sender, EventArgs e)


            {
           
            ImageButton b = sender as ImageButton;

            Spesa s = b.BindingContext as Spesa;

           if (s.posizione.Latitude!=0 && s.posizione.Longitude != 0)
                {

                Position luogo = s.posizione;

                await PopupNavigation.Instance.PushAsync (new Mappa (luogo));

                }
         

            else
                await PopupNavigation.Instance.PushAsync(new PopUpGenerico(Lingue.Resources.Errore, Lingue.Resources.AttivaMappa, "categoria3.png",null));
         
            }

        //###########################################################################################################################################################


        async void Edita_Spesa (object sender, EventArgs e)
            {

            ImageButton ib = sender as ImageButton;

            Spesa s = ib.BindingContext as Spesa;

            await PopupNavigation.Instance.PushAsync (new FormSpesa (s));


            }

        async void ScegliOrdine(object sender, EventArgs e)
            {
            string scelta = await DisplayActionSheet (Lingue.Resources.ORDINA, null, Lingue.Resources.ANNULLA, Lingue.Resources.DATA, Lingue.Resources.SOMMA);

            if (!scelta.Equals (Lingue.Resources.ANNULLA))

                SpeseViewModel.Instance.Ordine = scelta;
            }
        }
    }


