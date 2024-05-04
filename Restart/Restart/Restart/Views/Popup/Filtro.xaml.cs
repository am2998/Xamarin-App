using ReactiveUI;
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

    public partial class Filtro : Rg.Plugins.Popup.Pages.PopupPage
        {

        public Filtro ( )
            {

            InitializeComponent ( );

            this.BindingContext = SpeseViewModel.Instance;

            }



        async void FiltraLista (object sender, EventArgs e)
            {

            await PopupNavigation.Instance.PopAsync (true);

            }



        void AnnullaFiltraggio (object sender, EventArgs e )
            {

            SpeseViewModel.Instance.Somma = 2000;

            SpeseViewModel.Instance.Scelta = Lingue.Resources.Categoria;

            }

        }
    }

