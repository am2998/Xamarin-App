using ReactiveUI;
using Restart.Models;
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
    public partial class ListaSpeseNonComplete : Rg.Plugins.Popup.Pages.PopupPage
        {


        public Risparmio Risparmio { get; set; }

         public ListaSpeseNonComplete (Risparmio r)
            {
            InitializeComponent ( );


            this.BindingContext = CheckViewModel.Instance;

            Risparmio = r;
            }


         async void Imposta(object sender, SelectedItemChangedEventArgs e)
            {
           
            
            Spesa spesa = e.SelectedItem as Spesa;

                
                Spesa s = CheckViewModel.Instance.ListaSpese.Items.Where (x => x.ID == Risparmio.Spesa).FirstOrDefault ( );
                
                if (s != null)
                    {

                    s.TotaleRisparmi -= Risparmio.Somma;
                    s.Differenza = s.Somma - s.TotaleRisparmi;
               
                await App._cosmosServiceSpese.Salva(s, s.ID);


                    }

            
                Risparmio.Spesa = spesa.ID;

                CheckViewModel.Instance.Salva_Spesa.Execute (spesa);
               // CheckViewModel.Instance.Salva_Spesa.Execute (s);

                MessageBus.Current.SendMessage<Spesa> (spesa, "SpesaAssegnata");
                CheckViewModel.Instance.Salva_Risparmio.Execute (Risparmio).Subscribe ( );

                


                await PopupNavigation.Instance.PopAsync (true);

                
            }

        }
    }