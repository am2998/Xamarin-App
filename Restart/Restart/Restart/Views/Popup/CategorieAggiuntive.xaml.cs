using Restart.ViewModels;
using Rg.Plugins.Popup.Services;
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
    public partial class CategorieAggiuntive : Rg.Plugins.Popup.Pages.PopupPage
        {

        AltroViewModel vm = new AltroViewModel ("Categorie");

        public CategorieAggiuntive ( )
            {
            InitializeComponent ( );

            BindingContext = vm;
            }

        }
    }