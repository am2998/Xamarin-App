
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Restart.Services;
using Restart.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Restart.Views
    {
    [XamlCompilation (XamlCompilationOptions.Compile)]
    public partial class Intro : ContentPage
        {
        public Intro ( )
            {
            InitializeComponent ( );
            this.BindingContext = new IntroViewModel ( );

            }





        async void Apri (object sender, EventArgs e)
            {
 
            await Shell.Current.GoToAsync ("Login");

            }
        }
    }

