using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Reactive;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Restart.Models;
using Restart.Services;
using Restart.ViewModels;
using Restart.Views;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Restart.ViewModels
    {
    public sealed class SocialLoginPageViewModel : ReactiveObject
        {

        
        public ICommand OnLoginCommand { get; set; }

       
        private static readonly SocialLoginPageViewModel instance = new SocialLoginPageViewModel ( );
      
        static SocialLoginPageViewModel ( )
            {
            }


        [Reactive]
        public string Email { get; set; } = "";

        [Reactive]
        public bool IsBusy { get; set; } = false;



        [Reactive]
        public string Password { get; set; } = "";
        [Reactive]
        public string PasswordCopia { get; set; } = "";


        [Reactive]
        public string CodiceInviato { get; set; } = "";

        [Reactive]
        public string Codice { get; set; } = "";


        [Reactive]
        public bool IsShowCancel { get; set; }


        [Reactive]
        public string Check1 { get; set; } = "multiply.png";
        [Reactive]
        public string Check2 { get; set; } = "multiply.png";
        [Reactive]
        public string Check3 { get; set; } = "multiply.png";




        #region Commands



        private ICommand _cancelLoginCommand;
        public ICommand CancelLoginCommand
        {
            get { return _cancelLoginCommand = _cancelLoginCommand ?? new Command(CancelLoginAction); }
        }

       

        public ReactiveCommand<Unit, Unit> EffettuaRegistrazione { get; private set; }

        public ReactiveCommand<Unit, Unit> RecuperaPasswordDimenticata { get; private set; }

        public ReactiveCommand<Unit,Unit> Registra { get; private set; }

        public ReactiveCommand<Unit, Unit> Login { get; }

        public ReactiveCommand<Unit, Unit> ConfermaNuovaPassword { get; }



        public static ICosmosDBService _cosmosService { get; set; }



            #endregion

            #region Methods

   

        void CancelLoginAction()
        {
            
            IsBusy = false;


            IsShowCancel = false;
        }

     
        #endregion

         
        
        private SocialLoginPageViewModel ()
            {

            _cosmosService = new CosmosDBService ("Utenti");


            var accessoConsentito = this.WhenAnyValue (x => x.Password,x => x.PasswordCopia, (password, copia) => password.Any (char.IsUpper) && password.Any (char.IsLower) && password.Any (c => !char.IsLetterOrDigit (c)) && password.Any (char.IsDigit)
                     && password.Length > 9 && !string.IsNullOrEmpty (Email) && password.Equals(copia));


            this.WhenAnyValue (x => x.Password).Subscribe (password => {
                if (password.Any (char.IsUpper))
                    Check1 = "check.png";

                else
                    Check1 = "multiply.png";

                if (password.Any (c => !char.IsLetterOrDigit (c)))
                    Check2 = "check.png";

                else
                    Check2 = "multiply.png";

                if (password.Any (char.IsDigit))
                    Check3 = "check.png";

                else
                    Check3 = "multiply.png";


            });


            EffettuaRegistrazione = ReactiveCommand.CreateFromTask (async ( ) =>
             {

                 Password = "";

                 await Shell.Current.GoToAsync ("Login/Registrazione");

             });



            RecuperaPasswordDimenticata = ReactiveCommand.CreateFromTask (async ( ) =>
             {
               
                 IsBusy = true;

                 if (!string.IsNullOrEmpty (Email))
                     {

                     List<Utente> utenti = await _cosmosService.OttieniUtenteRegistrato (Email);

                     if (utenti != null && utenti.Count == 0)
                         {

                         IsBusy = false;

                         await PopupNavigation.Instance.PushAsync (new PopUpGenerico (Lingue.Resources.Errore, Lingue.Resources.AccountNonPresente, "categoria3.png", null));
                         await Shell.Current.GoToAsync ("Login/Registrazione");
                         Password = "";

                         }

                     else
                         {

                         Random r = new Random ( );

                         CodiceInviato = r.Next (100000, 999999).ToString ( );


                         MailMessage mail = new MailMessage ( );
                         mail.To.Add (Email);
                         mail.From = new MailAddress ("o2m.help@gmail.com");
                         mail.Subject = "";

                         mail.Body = Lingue.Resources.Codice + " " + CodiceInviato.ToString ( );
                         mail.IsBodyHtml = true;

                         SmtpClient smtp = new SmtpClient ( );
                         smtp.Host = "smtp.gmail.com";
                         smtp.Port = 587;
                         smtp.Credentials = new System.Net.NetworkCredential ("o2m.help@gmail.com", "Ftsmixtape98");

                         smtp.UseDefaultCredentials = false;

                         smtp.EnableSsl = true;
                         smtp.Send (mail);

                         IsBusy = false;

                         await PopupNavigation.Instance.PushAsync (new PopUpGenerico (Lingue.Resources.EmailInviata, Lingue.Resources.InserisciCodice, "categoria3.png", null));


                         await Shell.Current.GoToAsync ("Login/ResetPassword");
                         }
                     }

                 else
                     {
                     IsBusy = false;
                     await PopupNavigation.Instance.PushAsync (new PopUpGenerico (Lingue.Resources.Errore, Lingue.Resources.EmailValida, "categoria3.png", null));
                     }
             });



            Login = ReactiveCommand.CreateFromTask (async ( ) =>
            {
                 IsBusy = true;

                List<Utente> utenti = await _cosmosService.OttieniUtenteRegistrato (Email);



                if (utenti.Count != 0 && AesCrypt.Decrypt (utenti.FirstOrDefault ( ).Password, "passKey").Equals (Password))
                    {


                    Preferences.Set ("Login_Access", true);

                    var id = utenti.FirstOrDefault ( ).ID;

                    App._userID = id;

                    InizializzaAccount (id);

                    }

                else
                    {

                    await Task.Delay (3000).ContinueWith (_ => IsBusy = false);
                    await PopupNavigation.Instance.PushAsync (new PopUpGenerico (Lingue.Resources.Errore, Lingue.Resources.CredenzialiErrate, "categoria3.png",null));

                    Password = "";


                    }

            });

          
               
         
            Registra = ReactiveCommand.CreateFromTask (async ( ) =>
             {

                 IsBusy = true;

                 List<Utente> utenti = await _cosmosService.OttieniUtenteRegistrato (Email);

                 if (utenti != null && utenti.Count != 0)
                     {

                     IsBusy = false;

                     await PopupNavigation.Instance.PushAsync (new PopUpGenerico (Lingue.Resources.Errore, Lingue.Resources.AccountEsistente, "categoria3.png",null));
                     Password = "";
                     PasswordCopia = "";

                   
                    
                     }

                 else
                     {
                     Preferences.Set ("Login_Access", true);

                     var key = AesCrypt.Encrypt (Password, "passKey");

                     var _id = Guid.NewGuid ( ).ToString ( );

                     var temp = _id.Substring (0, 12);

                     Preferences.Set ("Iduser", AesCrypt.Encrypt (temp, "passId"));

                     Utente utente = new Utente { ID = _id, Email = Email, Password = key };

                     await _cosmosService.Salva (utente, utente.ID);

                     App._userID = _id;

                     InizializzaAccount (_id);

                     }

             

             },accessoConsentito);



           
            ConfermaNuovaPassword = ReactiveCommand.CreateFromTask (async ( ) =>
              {

                  IsBusy = true;


                  if (Codice.Equals (CodiceInviato) && !string.IsNullOrEmpty (Password) && Password.Any (char.IsUpper) && Password.Any (char.IsLower) && Password.Any (c => !char.IsLetterOrDigit (c)) && Password.Any (char.IsDigit)
                    && Password.Length >= 10 && !string.IsNullOrEmpty (Email) && Password.Equals(PasswordCopia))
                      {

                      List<Utente> utenti = await _cosmosService.OttieniUtenteRegistrato (Email);

                      if (utenti != null && utenti.Count != 0)
                          {

                          var u = utenti.FirstOrDefault ( );

                          u.Password = AesCrypt.Encrypt (Password, "passKey");

                          await _cosmosService.Salva (u, u.ID);

                          App._userID = u.ID;

                          InizializzaAccount (u.ID);

                          }

                      }

                  else
                      {

                      IsBusy = false;

                      await PopupNavigation.Instance.PushAsync (new PopUpGenerico (Lingue.Resources.Errore, Lingue.Resources.InserisciDatiCorretti, "categoria3.png",null));
                      Password = "";
                      PasswordCopia = "";
                      }


              });

            }


        public static SocialLoginPageViewModel Instance
            {
            get
                {
                return instance;
                }
            }


        public async void InizializzaAccount (string id)
            {

            App.InizializzaSpese (id);

            SpeseViewModel.Instance.CaricaSpese.Execute ().Subscribe ( );

            CheckViewModel.Instance.CaricaListe.Execute ( ).Subscribe ( );

            await Shell.Current.GoToAsync ("//HomePage").ContinueWith (_ => IsBusy = false);


            HomePageViewModel.Instance.ApriQuotaGiornaliera.Execute ( ).Subscribe ( );
            }


        }
    }
      