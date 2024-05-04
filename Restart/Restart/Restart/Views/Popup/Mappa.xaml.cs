using Restart.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Restart.Views
    {
    [XamlCompilation (XamlCompilationOptions.Compile)]
    public partial class Mappa : Rg.Plugins.Popup.Pages.PopupPage
        {


   

        public Mappa (Position p)
            {
            InitializeComponent ( );

            this.BindingContext = new MappaViewModel (p);

            Map.MapPosition = p;

            
            }


        async void Esci(object sender, EventArgs e)
            {
            await PopupNavigation.Instance.PopAsync (true);
            }
        }
    }