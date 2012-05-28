using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Device.Location;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Phone.Controls.Maps;

namespace GTL
{
    public partial class MainPage : PhoneApplicationPage
    {

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("DEBUGGER");
            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            App.ViewModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ViewModel_PropertyChanged);
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            Zoom();
            AddLocations();
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            App.ViewModel.LoadData();
        }

        private void SeeMore_Tap(object sender, EventArgs e)
        {
            string category = (sender as TextBlock).Name;
            NavigationService.Navigate(new Uri("/Pages/CategoriesPage.xaml?category="+category , UriKind.Relative));
        }

        private void Zoom() {
            LocationController.getCurrent((GeoCoordinate location) => {
                Map.SetView(location, 12);
            });
        }

        private void AddLocations() {
            MapLayer imageLayer = new MapLayer();
            
            foreach (CategoryViewModel category in App.ViewModel.Categories) {
                if (category.Venues == null) continue;
                foreach (VenueViewModel venue in category.Venues) {
                    Image image = new Image();
                    //Define the URI location of the image
                    image.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(venue.Icon, UriKind.Absolute));
                    //Define the image display properties
                    image.Opacity = 0.8;
                    image.Stretch = System.Windows.Media.Stretch.None;

                    //Add the image to the defined map layer
                    imageLayer.AddChild(image, venue.Location, PositionOrigin.Center);
                    //Add the image layer to the map
                }
            }

            Map.Children.Add(imageLayer);
        }
    }
}