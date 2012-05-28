using System;
using System.Globalization;
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
using Newtonsoft.Json.Linq;
using System.Device.Location;
using System.Collections.ObjectModel;


namespace GTL
{
    public class VenueViewModel : INotifyPropertyChanged
    {
        private string _id;
        public string Id {
            get { return _id; }
            set { 
                if (value != _id) {
                    _id = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }
        
        private string _name;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private string _address;
        public string Address {
            get { return _address; }
            set {
                if (value != _address)
                {   
                    _address = value;
                    NotifyPropertyChanged("Address");
                }
            }
        }

        private string _city;
        public string City {
            get { return _city; }
            set {
                if (value != _city)
                {
                    _city = value;
                    NotifyPropertyChanged("City");
                }
            }
        }

        private string _state;
        public string State
        {
            get { return _state; }
            set
            {
                if (value != _state)
                {
                    _state = value;
                    NotifyPropertyChanged("State");
                }
            }
        }

        private string _country;
        public string Country
        {
            get { return _country; }
            set
            {
                if (value != _country)
                {
                    _country = value;
                    NotifyPropertyChanged("Country");
                }
            }
        }

        private string _icon;
        public string Icon
        {
            get { return _icon; }
            set
            {
                if (value != _icon)
                {
                    _icon = value;
                    NotifyPropertyChanged("Icon");
                }
            }
        }

        private GeoCoordinate _location;
        public GeoCoordinate Location {
            get { return _location; }
            set {
                if (value != _location) {
                    _location = value;
                    NotifyPropertyChanged("Location");
                }
            }
        }

        private string _picture;
        public string Picture {
            get { return _picture; }
            set {
                if (value != _picture) {
                    _picture = value;
                    NotifyPropertyChanged("Picture");
                }
            }
        }

        public ObservableCollection<TipViewModel> Tips { get; set; }
        public ObservableCollection<PhotoViewModel> Photos { get; set; }
        public string CityState { get; set; }
        public string NameCaps { get; set; }
        public double Distance { get; set; }

        public static VenueViewModel CreateFromJson(JObject json) {
            VenueViewModel venue = new VenueViewModel() {
                Id =        (string)json["id"],
                Name =      (string)json["name"],
                Address =   (string)json["location"]["address"],
                City =      (string)json["location"]["city"],
                State =     (string)json["location"]["state"],
                CityState = (string)json["location"]["city"] + ", " + (string) json["location"]["state"],
                Country =   (string)json["location"]["country"],
                Location = new GeoCoordinate() {
                    Latitude =  (double)json["location"]["lat"],
                    Longitude = (double)json["location"]["lng"]
                },
                Icon =    (string) json["categories"][0]["icon"]["prefix"] +"32.png",
                Picture = (string) json["categories"][0]["icon"]["prefix"] +"88.png"

            };

            if (json["location"]["distance"] != null) {
                venue.Distance = (double)json["location"]["distance"];
            }

            return venue;
        }

        public static VenueViewModel CreateFromBigJson(JObject json) {
            VenueViewModel venue = VenueViewModel.CreateFromJson(json);

            venue.NameCaps = venue.Name.ToUpper();

            ObservableCollection<TipViewModel> tips = new ObservableCollection<TipViewModel>();
            if (json["tips"]["groups"][0]["items"] != null) {
                foreach (JObject tip in json["tips"]["groups"][0]["items"]) {
                    tips.Add(new TipViewModel() {
                        Text = (string)tip["text"],
                        User = (string)tip["user"]["firstName"],
                        Photo = (string)tip["user"]["photo"],
                        Date = (int)tip["createdAt"]
                    });
                }
            }
            venue.Tips = tips;

            ObservableCollection<PhotoViewModel> photos = new ObservableCollection<PhotoViewModel>();
            if (json["photos"]["groups"][1]["items"] != null) {
                foreach (JObject photo in json["photos"]["groups"][1]["items"]) {
                    Debug.WriteLine((string)photo["url"]);
                    photos.Add(new PhotoViewModel() {
                        Photo = (string) photo["url"]   
                    });
                }
            }
            venue.Photos = photos;


            return venue;
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