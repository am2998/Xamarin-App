
using Restart.Services;
using Restart.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Restart.Views
    {
    [XamlCompilation (XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
        {



        public Login ( )
            {

            InitializeComponent ( );
            this.BindingContext = SocialLoginPageViewModel.Instance;
            }


        void MostraPassword (object sender, EventArgs e)
            {

            Plugin.InputKit.Shared.Controls.CheckBox cb = sender as Plugin.InputKit.Shared.Controls.CheckBox;

            if (cb.IsChecked)
                {
                Password.IsPassword = false;
               
                }

            else
                {
                Password.IsPassword = true;
         
                }

            }

        }
    }