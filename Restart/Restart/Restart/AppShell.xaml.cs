using Restart.ViewModels;
using Restart.Views;
using Restart.Views.PagineIntro;
using System;
using System.Collections.Generic;


using Xamarin.Forms;

namespace Restart
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            //Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute ("Impostazioni", typeof (PaginaImpostazioni));
            Routing.RegisterRoute ("Donazione", typeof (Donazione));
            Routing.RegisterRoute ("Assistenza", typeof (Assistenza));
            Routing.RegisterRoute ("Intro", typeof (Intro));
            Routing.RegisterRoute ("HomePage", typeof (HomePage));
            Routing.RegisterRoute ("Login", typeof (Login));
            Routing.RegisterRoute ("Login/Registrazione", typeof (PaginaRegistrazione));
            Routing.RegisterRoute ("Login/ResetPassword", typeof (PaginaRecuperoPassword));





            CurrentItem = StartPage;
            
        }

    }
}
