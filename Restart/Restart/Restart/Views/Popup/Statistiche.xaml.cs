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
    public partial class Statistiche : Rg.Plugins.Popup.Pages.PopupPage
        {
        StatisticheViewModel vm;

        public Statistiche ( )
            {
            InitializeComponent ( );
            this.BindingContext = vm = new StatisticheViewModel ( );
            }


        async void Esci(object sender, EventArgs e)
            {
            await PopupNavigation.Instance.PopAsync (true);
            }
        }
    }