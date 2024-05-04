using Restart.Models;
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
    public partial class FormQuotaGiornaliera : Rg.Plugins.Popup.Pages.PopupPage
        {

        FormQuotaGiornalieraViewModel vm;

        public FormQuotaGiornaliera(double SpeseOggi, double SpeseIeri)
            {

            InitializeComponent ( );

            vm = new FormQuotaGiornalieraViewModel (SpeseOggi, SpeseIeri);

            this.BindingContext = vm;

            Salva.CommandParameter = true;

            }
        }
    }