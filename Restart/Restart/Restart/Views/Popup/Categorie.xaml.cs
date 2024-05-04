using Restart.ViewModels;
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
    public partial class Categorie : Rg.Plugins.Popup.Pages.PopupPage
        {
        

        public Categorie (string tipo)
            {
            InitializeComponent ( );
            BindingContext = new AltroViewModel(tipo);
            }



            
        }
    }