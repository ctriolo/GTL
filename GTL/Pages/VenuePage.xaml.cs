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
using Microsoft.Phone.Controls.Maps;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Microsoft.Phone.Tasks;

namespace GTL {
    public partial class VenuePage : PhoneApplicationPage {
        public VenuePage() {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e) {
            base.OnNavigatedTo(e);
            string id = "";
            if (NavigationContext.QueryString.TryGetValue("id", out id)) {
                FourSquareAPI.Venue(id, (JObject json_venue) => {
                    VenueViewModel venue = VenueViewModel.CreateFromBigJson(json_venue);
                    Deployment.Current.Dispatcher.BeginInvoke(() => {
                        DataContext = venue;
                        AddLocation();
                        Debug.WriteLine(venue.Id);  
                    });
                });
            }
        }

        private void AddLocation() {
            MapLayer imageLayer = new MapLayer();

            VenueViewModel venue = DataContext as VenueViewModel;
                    Image image = new Image();
                    //Define the URI location of the image
                    image.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(venue.Icon, UriKind.Absolute));
                    //Define the image display properties
                    image.Opacity = 0.8;
                    image.Stretch = System.Windows.Media.Stretch.None;

                    //Add the image to the defined map layer
                    imageLayer.AddChild(image, venue.Location, PositionOrigin.Center);
                    //Add the image layer to the map

            map.Children.Add(imageLayer);
        }

        private void map_Tap(object sender, System.Windows.Input.GestureEventArgs e) {
            BingMapsDirectionsTask bingMapsDirectionsTask = new BingMapsDirectionsTask();
            VenueViewModel venue = DataContext as VenueViewModel;
            bingMapsDirectionsTask.End = new LabeledMapLocation(venue.Name, venue.Location);
            bingMapsDirectionsTask.Show();
        }
    }
}