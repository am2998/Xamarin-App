using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Restart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;

namespace Restart.ViewModels
    {

    public class StatisticheViewModel
        {

        [Reactive]
        public double SpesaMassima { get; set; }

        [Reactive]
        public double SpeseDelMese { get; set; }

        [Reactive]
        public double SpeseDelMesePrecedente { get; set; }

        [Reactive]
        public double NumeroSpese { get; set; }

        [Reactive]
        public double SpeseSettimana { get; set; }


        public ReactiveCommand<Unit,Unit> Calcola { get; }


        public StatisticheViewModel ( )
            {
            Calcola = ReactiveCommand.Create (( ) =>
             {
      
                 SpeseDelMese = 0;

                 SpeseDelMesePrecedente = 0;

                 SpeseSettimana = 0;

                 DateTime d = DateTime.Today;

                 NumeroSpese = 0;

                 int settimana = -1;



                 if (d.Day >= 1 && d.Day <= 7)
                     settimana = 1;
                 if (d.Day >= 8 && d.Day <= 14)
                     settimana = 2;
                 if (d.Day >= 15 && d.Day <= 21)
                     settimana = 3;
                 if (d.Day >= 22 && d.Day <= 31)
                     settimana = 4;

                 if (SpeseViewModel.Instance.ListaSpese.Count != 0)
                     {
                     Spesa spesa = SpeseViewModel.Instance.ListaSpese.Items.OrderByDescending (x => x.Somma).FirstOrDefault ( );
                     SpesaMassima = spesa.Somma;
                     }

                 foreach (Spesa s in SpeseViewModel.Instance.ListaSpese.Items)
                     {

                     if (s.Data.Month == DateTime.Today.Month)
                         SpeseDelMese += s.Somma;
                     else if (s.Data.Month == DateTime.Today.Month - 1)

                         SpeseDelMesePrecedente += s.Somma;


                     NumeroSpese++;

                     if (settimana == 1 && s.Data.Day >= 1 && s.Data.Day <= 7)
                         SpeseSettimana += s.Somma;
                     if (settimana == 2 && s.Data.Day >= 8 && s.Data.Day <= 14)
                         SpeseSettimana += s.Somma;
                     if (settimana == 3 && s.Data.Day >= 15 && s.Data.Day <= 21)
                         SpeseSettimana += s.Somma;
                     if (settimana == 4 && s.Data.Day >= 22 && s.Data.Day <= 31)
                         SpeseSettimana += s.Somma;

                     }
                 });

            Calcola.Execute ( ).Subscribe ( );
            }

        }
    }
