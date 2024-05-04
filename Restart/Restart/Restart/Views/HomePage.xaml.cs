using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Restart.ViewModels;
using Restart.Models;
using ReactiveUI;
using ReactiveUI.XamForms;
using Rg.Plugins.Popup.Services;
using Plugin.LocalNotifications;
using Xamarin.Essentials;
using System.Threading;
using ReactiveUI.Fody.Helpers;
using DynamicData;
using Xamarin.Forms.Maps;

namespace Restart.Views
    {
    [XamlCompilation (XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
        {
 

        public HomePage ( )
            { 

            InitializeComponent ( );
            BindingContext = HomePageViewModel.Instance;
           

            }
        

        CancellationTokenSource cts;

        async Task<Location> OttieniPosizioneCorrente ( )
            {
            try
                {
                var request = new GeolocationRequest (GeolocationAccuracy.Best, TimeSpan.FromSeconds (10));
                cts = new CancellationTokenSource ( );
                var location = await Geolocation.GetLocationAsync (request, cts.Token);

                return location;

                }
            catch (FeatureNotSupportedException)
                {
                await PopupNavigation.Instance.PushAsync (new PopUpGenerico (Lingue.Resources.Errore, Lingue.Resources.NoSupporto, "categoria3.png", null));
               
                return null;
                // Handle not supported on device exception
                }
            catch (FeatureNotEnabledException)
                {
                await PopupNavigation.Instance.PushAsync (new PopUpGenerico (Lingue.Resources.Errore, Lingue.Resources.NoAttivato, "categoria3.png", null));
                return null;
                // Handle not enabled on device exception
                }
            catch (PermissionException)
                {
                await PopupNavigation.Instance.PushAsync (new PopUpGenerico (Lingue.Resources.Errore, Lingue.Resources.NoPermessi, "categoria3.png", null));
                return null;
                // Handle permission exception
                }
            catch (Exception)
                {
                await PopupNavigation.Instance.PushAsync (new PopUpGenerico (Lingue.Resources.Errore, Lingue.Resources.NonTrovo, "categoria3.png", null));
                return null;
                // Unable to get location
                }
            }

        protected override void OnDisappearing ( )
            {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel ( );
            base.OnDisappearing ( );
            }



        protected async override void OnAppearing ( )
            {
            base.OnAppearing ( );


            var current = Connectivity.NetworkAccess;

            if (current != NetworkAccess.Internet)

                await PopupNavigation.Instance.PushAsync (new PopUpGenerico (Lingue.Resources.Errore, Lingue.Resources.NoConnessione, "categoria3.png", "Connessione"));

            }



        async void Altro (object sender, EventArgs e)
            {
         

            string pagina = await DisplayActionSheet ("Altro:", null, Lingue.Resources.INDIETRO, Lingue.Resources.CategorieAggiuntive);

           

            if (pagina.Equals (Lingue.Resources.CategorieAggiuntive))

                await PopupNavigation.Instance.PushAsync (new CategorieAggiuntive ( ));

       
            }


        //#########################################################################################################################################################################


        async void Impostazioni (object sender, EventArgs e)
            {
            string scelta = await DisplayActionSheet ("", Lingue.Resources.INDIETRO, null, Lingue.Resources.IMPOSTAZIONI, Lingue.Resources.DONAZIONE, Lingue.Resources.ASSISTENZA, Lingue.Resources.LogOut);


            if (scelta.Equals (Lingue.Resources.IMPOSTAZIONI))

                await Shell.Current.GoToAsync ("Impostazioni");
                


            if (scelta.Equals (Lingue.Resources.DONAZIONE))

                await Shell.Current.GoToAsync ("Donazione");

            if (scelta.Equals (Lingue.Resources.ASSISTENZA))

                await Shell.Current.GoToAsync ("Assistenza");

      

            if (scelta.Equals (Lingue.Resources.LogOut))
                {
                Preferences.Set ("Login_Access", false);

                await Shell.Current.GoToAsync ("Login");

                }


            }


        //#########################################################################################################################################################################

  
        
         async void Salva (object sender, EventArgs e)
            {

            bool Completa = false;



            if (( Convert.ToDouble (Cifra.Text) ) == 0)

                await PopupNavigation.Instance.PushAsync (new PopUpGenerico (Lingue.Resources.Errore, Lingue.Resources.INSERISCI_CIFRA, "categoria3.png",null));

            else if (Scelta.Text.Equals(Lingue.Resources.Categoria))
                await PopupNavigation.Instance.PushAsync (new PopUpGenerico (Lingue.Resources.Errore, Lingue.Resources.INSERISCI_CATEGORIA, "categoria3.png", null));


            else
                {

                Location luogo = null;

                string scelta = await DisplayActionSheet (Lingue.Resources.SELEZIONA_SPESA, Lingue.Resources.ANNULLA, null, Lingue.Resources.SPESA_EFFETTUATA, Lingue.Resources.SPESA_DAEFFETTUARE);

                if (scelta.Equals (Lingue.Resources.SPESA_EFFETTUATA))

                    Completa = true;




                Spesa s = new Spesa
                    {
                    ID = Guid.NewGuid ( ).ToString ( ),
                    Somma = Convert.ToDouble (Cifra.Text),
                    Categoria = Convert.ToString (Scelta.Text),
                    Completa = Completa,
                    Data = DateTime.Now,
                    TotaleRisparmi = 0,
                    Differenza = Convert.ToDouble (Cifra.Text),
                    posizione = new Position (0, 0),
                    Chiave = App._userID

                    };




                if (Preferences.Get("Localizzazione",false))
                    {
                    luogo = await OttieniPosizioneCorrente ( );

                    if (luogo != null)
                        {
                        s.posizione = new Position(luogo.Latitude, luogo.Longitude);

                        if (s.Completa)

                            HomePageViewModel.Instance.SalvaSpesa.Execute (s).Subscribe ( );

                        else

                            MessageBus.Current.SendMessage<Spesa> (s, "NonCompleta");
                       

                        }

                    }

                 else if (s.Completa)

                    HomePageViewModel.Instance.SalvaSpesa.Execute (s).Subscribe ( );

                else

                    MessageBus.Current.SendMessage<Spesa> (s, "NonCompleta");
                    
                    

                Aumenta.Value = 0;
                HomePageViewModel.Instance.CategoriaSelezionata = Lingue.Resources.Categoria;


                }
            }





        //#########################################################################################################################################################################



       



        async void Edita (object sender, EventArgs e)
            {

            ImageButton b = sender as ImageButton;

          
            Spesa s = b.BindingContext as Spesa;

            await PopupNavigation.Instance.PushAsync (new FormSpesa(s));


            }
  






        //#########################################################################################################################################################################


        void Elimina (object sender, EventArgs e)
           
            {

            
            ImageButton ib = sender as ImageButton;

            Spesa s = ib.BindingContext as Spesa;

            HomePageViewModel.Instance.Elimina.Execute (s);
            


            }





         async void SalvaQuota (object sender, EventArgs e)
            {

            ImageButton a = sender as ImageButton;


            string azione = await DisplayActionSheet(Lingue.Resources.GestisciQuota, null, Lingue.Resources.INDIETRO, Lingue.Resources.Giornaliera, Lingue.Resources.AScelta, Lingue.Resources.Mensile);

            if (azione.Equals (Lingue.Resources.Giornaliera))
                await PopupNavigation.Instance.PushAsync (new FormQuotaGiornaliera(HomePageViewModel.Instance.QuantitaOggi,HomePageViewModel.Instance.QuantitaIeri));

            else if (azione.Equals (Lingue.Resources.AScelta))

                await PopupNavigation.Instance.PushAsync (new FormQuota("A scelta",HomePageViewModel.Instance.TotaleSpese));

            else if (azione.Equals (Lingue.Resources.Mensile))

                await PopupNavigation.Instance.PushAsync (new FormQuota ("Mensile",HomePageViewModel.Instance.TotaleSpese));


            }
        }
    }

 



 