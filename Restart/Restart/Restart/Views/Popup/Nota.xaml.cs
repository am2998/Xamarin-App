using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace Restart.Views
    {
    [XamlCompilation (XamlCompilationOptions.Compile)]
    public partial class Nota : Rg.Plugins.Popup.Pages.PopupPage
        {
        public Nota ( )
            {
            InitializeComponent ( );
            }

        async void Chiudi(object sender, EventArgs e)
            {
            await PopupNavigation.Instance.PopAsync (true);
            }
        }
    }