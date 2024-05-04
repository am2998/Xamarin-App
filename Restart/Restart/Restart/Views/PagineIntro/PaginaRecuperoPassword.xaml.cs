using Restart.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Restart.Views.PagineIntro
    {
    [XamlCompilation (XamlCompilationOptions.Compile)]
    public partial class PaginaRecuperoPassword : ContentPage
        {
        public PaginaRecuperoPassword ( )
            {
            InitializeComponent ( );
            this.BindingContext = SocialLoginPageViewModel.Instance;
            }


        async void Indietro (object sender, EventArgs e)
            {
            await Shell.Current.GoToAsync ("Login");
            SocialLoginPageViewModel.Instance.Password = "";
            SocialLoginPageViewModel.Instance.PasswordCopia = "";

            }

        void MostraPassword (object sender, EventArgs e)
            {

            Plugin.InputKit.Shared.Controls.CheckBox cb = sender as Plugin.InputKit.Shared.Controls.CheckBox;

            if (cb.IsChecked)
                {
                Password.IsPassword = false;
                PasswordCopia.IsPassword = false;
                }

            else
                {
                Password.IsPassword = true;
                PasswordCopia.IsPassword = true;
                }

                
            }
        }
    }