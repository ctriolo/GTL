using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Device.Location;

namespace GTL
{
    public class CategoryViewModel : INotifyPropertyChanged
    {
        private string _name;
        public string Name {
            get { return _name; }
            set {
                if (value != _name) {
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private ObservableCollection<VenueViewModel> _venues;
        public  ObservableCollection<VenueViewModel> Venues {
            get { return _venues; }
            set {
                if (value != _venues) {
                    _venues = value;
                    NotifyPropertyChanged("Venues");
                }
            }
        }

        public GeoCoordinate Location { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}