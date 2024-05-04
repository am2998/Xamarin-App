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
    public partial class Donazione : ContentPage
        {
        public Donazione ( )
            {
            InitializeComponent ( );
            }


        void Apri(object sender, EventArgs e)
            {
            Device.OpenUri (new Uri ("https://paypal.me/o2mA?locale.x=it_IT"));
            }
        }
    }