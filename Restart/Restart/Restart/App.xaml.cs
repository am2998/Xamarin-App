using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using Plugin.LocalNotifications;
using ReactiveUI;
using Restart.Lingue;
using Restart.Models;
using Restart.Services;
using Restart.ViewModels;
using Restart.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;



[assembly: ExportFont("Lobster-Regular.ttf", Alias = "Lobster")]
[assembly: ExportFont ("times new roman.ttf", Alias = "TNR")]

namespace Restart
    {
    public partial class App : Application
        {



        public static ICosmosDBService _cosmosServiceUser { get; set; }
        public static ICosmosDBService _cosmosServiceSpese { get; set; }

        public static string _userID { get; set; }

        public App ( )
            {
            _cosmosServiceSpese = new CosmosDBService ("Spese");
            _cosmosServiceUser = new CosmosDBService ("Utenti");

            InitializeComponent ( );
            Device.SetFlags (new string[] { "Shapes_Experimental" });

            LocalizationResourceManager.Current.PropertyChanged += (_, lingua) => Lingue.Resources.Culture = LocalizationResourceManager.Current.CurrentCulture;
            LocalizationResourceManager.Current.Init (Lingue.Resources.ResourceManager);


            VersionTracking.Track ( );

            MainPage = new AppShell ( );

            }


        protected async override void OnStart ( )
            {
            await GestisciAccessoConImpronta ( );
            await GestisciAccessoUtente ( );
            }

        protected override void OnSleep ( )
            {
            }

        protected override void OnResume ( )
            {
            }



        //#####################################################################################

        public async static void InizializzaSpese (string userid)
            {
            var mostra = false;

            var spese = await _cosmosServiceSpese.OttieniSpese (userid);

            IEnumerable<Spesa> lista = spese.Where (x => x.Completa);

            if (lista != null && lista.Count ( ) == 0)
                mostra = true;

            MessageBus.Current.SendMessage<(bool, IEnumerable<Spesa>)> ((mostra, lista), "ListaPartenzaSpese");


            }

        public async Task GestisciAccessoConImpronta ( )
            {
            bool canPass = false;


            if (Preferences.Get ("Fingerprint", false) == true)
                {

                do
                    {

                    bool isFingerprintAvailable = await CrossFingerprint.Current.IsAvailableAsync (false);
                    if (!isFingerprintAvailable)
                        {
                        await Shell.Current.DisplayAlert (Lingue.Resources.Errore,
                           Lingue.Resources.ErroreAutenticazione, "OK");
                        return;
                        }

                    AuthenticationRequestConfiguration conf =
                        new AuthenticationRequestConfiguration (Lingue.Resources.Autenticazione,
                        Lingue.Resources.Accedi);

                    var authResult = await CrossFingerprint.Current.AuthenticateAsync (conf);

                    if (authResult.Authenticated == false)
                        {

                        await PopupNavigation.Instance.PushAsync (new PopUpGenerico (Lingue.Resources.Errore, Lingue.Resources.AutenticazioneFallita, "categoria3.png", null));
                        }

                    else
                        canPass = true;

                    } while (canPass == false);

                }

            }


        public async Task GestisciAccessoUtente ( )
            {

            try
                {

                var Euserid = Preferences.Get ("Iduser", null);

                if (Euserid != null)
                    {
                    if (Preferences.Get ("Login_Access", false) == false)


                        await Shell.Current.GoToAsync ("Login");

                    else
                        {
                        var Duserid = AesCrypt.Decrypt (Euserid, "passId");



                        List<Utente> listaUtenti = await _cosmosServiceUser.OttieniUtente (Duserid);

                        if (listaUtenti != null && listaUtenti.Count != 0)


                            _userID = listaUtenti.FirstOrDefault ( ).ID;



                        InizializzaSpese (_userID);


                        await Shell.Current.GoToAsync ("//HomePage");


                        HomePageViewModel.Instance.ApriQuotaGiornaliera.Execute ( ).Subscribe ( );

                        }

                    }

                else if (VersionTracking.IsFirstLaunchEver)


                    await Shell.Current.GoToAsync ("Intro");

                else
                    await Shell.Current.GoToAsync ("Login");

                }

            catch (Exception e)
                {
                await PopupNavigation.Instance.PushAsync (new PopUpGenerico (Lingue.Resources.Errore,Lingue.Resources.NoConnessione, "categoria3.png", "Connessione"));
                OnStart ( );

                }


            }
        }
    }