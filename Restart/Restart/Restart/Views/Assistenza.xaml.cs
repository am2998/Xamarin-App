using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Restart.Views
    {
    [XamlCompilation (XamlCompilationOptions.Compile)]
    public partial class Assistenza : ContentPage
        {
        public Assistenza ( )
            {
            InitializeComponent ( );



            }


        void Open (object sender, EventArgs e)
            {
            Device.OpenUri (new Uri ("mailto:o2m.help@gmail.com"));
            }
        }
    }