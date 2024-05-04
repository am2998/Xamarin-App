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
    public partial class PopUpGenerico : Rg.Plugins.Popup.Pages.PopupPage
        {


        public string Condizioni { get; set; }

        public PopUpGenerico (string titolo, string messaggio, string immagine, string parametriAggiuntivi)
            {
            InitializeComponent ( );

            Titolo.Text = titolo;
            Messaggio.Text = messaggio;
            Immagine.Source = immagine;
            Condizioni = parametriAggiuntivi;

            }

        public async void Esegui (object sender, EventArgs e)
            {
            if (Condizioni!=null  && Condizioni.Equals ("Connessione"))
                {

                var current = Connectivity.NetworkAccess;

                while (current != NetworkAccess.Internet)
                    {

                    current = Connectivity.NetworkAccess;

                    }

                }


            await PopupNavigation.Instance.PopAsync (true);
           
            }
        }
    }