using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace debug
{




    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaImpostazioni : ContentPage
    {
        public PaginaImpostazioni()
        {
            InitializeComponent();
            ImpostaAvvio();

            var list = new List<string>();
            list.Add(Properties.Resources.ITALIANO);
            list.Add(Properties.Resources.INGLESE);
            list.Add(Properties.Resources.SPAGNOLO);

            LinguaScelta.ItemsSource = list;

        }

        public static string LinguaSistema { get; set; }

        public static string TemaSistema { get; set; }





        async void ImpostaAvvio()
        {
            var i = await App.SQLiteDb.OttieniImpostazioni();

            LinguaScelta.SelectedIndex = -1;
            TemaAttivato.IsToggled = false;

            if (i != null)
            {

                if (i.Lingua.Equals("it-IT"))
                    LinguaScelta.SelectedIndex = 0;



                if (i.Lingua.Equals("en-US"))
                    LinguaScelta.SelectedIndex = 1;


                if (i.Lingua.Equals("es-ES"))
                    LinguaScelta.SelectedIndex = 2;


                
                


                /*

                     if (i.Tema.Equals("Scuro"))
                        TemaAttivato.IsToggled = true;

                    else
                        TemaAttivato.IsToggled = false
                */
            }

        }





        void CambiaTema(object sender, AppThemeChangedEventArgs e)
        {

            TemaAttivato = null;


            if (TemaAttivato.IsToggled)
            {

                Application.Current.UserAppTheme = OSAppTheme.Dark;
                TemaSistema = "Scuro";
            }




            else
            {

                Application.Current.UserAppTheme = OSAppTheme.Light;
                TemaSistema = "Chiaro";

            }

           

        }

        async void CambiaLingua(object sender, EventArgs e)
        {

            Picker a = sender as Picker;

            if (a.SelectedIndex == 0)

                LinguaSistema = "it-IT";

            if (a.SelectedIndex == 1)

                LinguaSistema = "en-US";

            if (a.SelectedIndex == 2)

                LinguaSistema = "es-ES";


            CultureInfo info = new CultureInfo(LinguaSistema);
            Properties.Resources.Culture = info;
            Thread.CurrentThread.CurrentUICulture = info;
            
            await App.SQLiteDb.SalvaImpostazioni(new Impostazioni
            {
                Lingua = LinguaSistema,
                Tema = null,
                ID = 2
            });

           
        }


        


        async void ApriMainpage(object sender, EventArgs e)
        {



            /*   await App.SQLiteDb.SalvaImpostazioni(new Impostazioni
              {
                  Lingua = LinguaSistema,
                  Tema = TemaSistema,
                  ID = 2
              });
            */



            await Navigation.PushAsync(new MainPage());
        }
    }

}