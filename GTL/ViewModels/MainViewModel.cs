using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Device.Location;
using Newtonsoft.Json.Linq;

namespace GTL
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public enum CATEGORIES {
            GYM = 0,
            TAN,
            LAUNDRY
        };

        public MainViewModel()
        {
            this.Categories = new ObservableCollection<CategoryViewModel>();
            this.Categories.Add(new CategoryViewModel() { Name = "no" });
            this.Categories.Add(new CategoryViewModel() { Name = "no" });
            this.Categories.Add(new CategoryViewModel() { Name = "no" });
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<CategoryViewModel> Categories { get; private set; }
        public GeoCoordinate Location { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        public void ClearData()
        {
            this.Categories = new ObservableCollection<CategoryViewModel>();
            NotifyPropertyChanged("Categories");
        }


        private bool isReady() {
            foreach (CategoryViewModel category in App.ViewModel.Categories) {
                if (category.Venues == null) return false;
            }
            return true;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {

            LocationController.getCurrent((GeoCoordinate location) => {

                this.Location = location;
                NotifyPropertyChanged("Location");

                // Gym
                FourSquareAPI.Search(location, FourSquareAPI.FS_CATEGORIES_GYM, (JArray json_venues) => {
                    Deployment.Current.Dispatcher.BeginInvoke(() => {
                        ObservableCollection<VenueViewModel> venues = new ObservableCollection<VenueViewModel>();
                        foreach (JObject json_venue in json_venues) {
                            venues.Add(VenueViewModel.CreateFromJson(json_venue));
                        }
                        this.Categories[(int) CATEGORIES.GYM] = new CategoryViewModel() {
                            Name = "gym",
                            Location = location,
                            Venues = venues
                        };
                        if (isReady()) NotifyPropertyChanged("Categories");
                    });
                });

                // Tan
                FourSquareAPI.Search(location, FourSquareAPI.FS_CATEGORIES_TAN, (JArray json_venues) => {
                    Deployment.Current.Dispatcher.BeginInvoke(() => {
                        ObservableCollection<VenueViewModel> venues = new ObservableCollection<VenueViewModel>();
                        foreach (JObject json_venue in json_venues) {
                            venues.Add(VenueViewModel.CreateFromJson(json_venue));
                        }
                        this.Categories[(int) CATEGORIES.TAN] = new CategoryViewModel() {
                            Name = "tan",
                            Location = location,
                            Venues = venues
                        };
                        if (isReady()) NotifyPropertyChanged("Categories");
                    });
                });

                // Laundry
                FourSquareAPI.Search(location, FourSquareAPI.FS_CATEGORIES_LAUNDRY, (JArray json_venues) => {
                    Deployment.Current.Dispatcher.BeginInvoke(() => {
                        ObservableCollection<VenueViewModel> venues = new ObservableCollection<VenueViewModel>();
                        foreach (JObject json_venue in json_venues) {
                            venues.Add(VenueViewModel.CreateFromJson(json_venue));
                        }
                        this.Categories[(int) CATEGORIES.LAUNDRY] = new CategoryViewModel() {
                            Name = "laundry",
                            Location = location,
                            Venues = venues
                        };
                        if (isReady()) NotifyPropertyChanged("Categories");
                    });
                });

            });
        }

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