using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms.Maps;

namespace Restart.ViewModels
    {
    public class MappaViewModel : BaseViewModel
        {
        private Position posizione = new Position (-37.8141, 144.9633);
        public Position Posizione { get { return posizione; } set { posizione = value; OnPropertyChanged ( ); } }

        private ObservableCollection<Pin> _pinCollection = new ObservableCollection<Pin> ( );
        public ObservableCollection<Pin> PinCollection
            {
            get { return _pinCollection; }
            set { _pinCollection = value; OnPropertyChanged ( ); }
            }

        public MappaViewModel (Position p) 
            {

            Posizione = p;
            PinCollection.Add (new Pin ( ) { Position = p, Type = PinType.Generic, Label = "I'm a Pin" });
            }

        }
    }
