using ReactiveUI.XamForms;
using Restart.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Restart.Views
    {
    [XamlCompilation (XamlCompilationOptions.Compile)]
    public partial class PaginaImpostazioni : ContentPage
        {

        ImpostazioniViewModel vm = new ImpostazioniViewModel ( );

        public PaginaImpostazioni ( )
            {

            InitializeComponent ( );
            BindingContext = vm;

            }


        async void CambiaLingua(object sender, EventArgs e)
            {
            string LinguaScelta = await DisplayActionSheet (Lingue.Resources.Lingue, null, Lingue.Resources.INDIETRO, "Italiano", "English", "Español", "French", "日本語", "Deutsch", "русский", "中文");

            if (!LinguaScelta.Equals (Lingue.Resources.INDIETRO))

                vm.Lingua = LinguaScelta;
            }



        }
    }