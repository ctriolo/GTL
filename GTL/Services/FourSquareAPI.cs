using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;
using System.Device.Location;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GTL
{
    public class FourSquareAPI
    {
        // Gym
        public const string FS_CATEGORY_COLLEGE_GYM = "4bf58dd8d48988d1b2941735";
        public const string FS_CATEGORY_GYM = "4bf58dd8d48988d175941735";
        public static string[] FS_CATEGORIES_GYM = { FS_CATEGORY_GYM, FS_CATEGORY_COLLEGE_GYM };
        
        // Tan
        public const string FS_CATEGORY_TANNING_SALON = "4d1cf8421a97d635ce361c31";
        public const string FS_CATEGORY_BEACH = "4bf58dd8d48988d1e2941735";
        public const string FS_CATEGORY_POOL = "4bf58dd8d48988d15e941735";
        public static string[] FS_CATEGORIES_TAN = { FS_CATEGORY_TANNING_SALON, FS_CATEGORY_BEACH, FS_CATEGORY_POOL };

        // Laundry
        public const string FS_CATEGORY_LAUNDRY = "4bf58dd8d48988d1fc941735";
        public static string[] FS_CATEGORIES_LAUNDRY = { FS_CATEGORY_LAUNDRY };

        // URLs
        const string FS_CLIENT_ID = KEYS.FOURSQUARE_APP_ID;
        const string FS_CLIENT_SECRET = KEYS.FOURSQUARE_APP_SECRET;
        const string FS_CLIENT_DATE = "20120428";
        const string FS_BASE = "https://api.foursquare.com/v2";
        const string FS_VENUE_SEARCH = FS_BASE +
            "/venues/search?ll={0},{1}&categoryId={2}" +
            "&client_id=" + FS_CLIENT_ID +
            "&client_secret=" + FS_CLIENT_SECRET +
            "&v=" + FS_CLIENT_DATE;
        const string FS_VENUE = FS_BASE +
            "/venues/{0}" +
            "?client_id=" + FS_CLIENT_ID  +
            "&client_secret=" + FS_CLIENT_SECRET +
            "&v=" + FS_CLIENT_DATE;

        private delegate void ResponseDelegate(JObject response);

        private static void GetRequest(string url, ResponseDelegate callback) {
            var initialRequest = HttpWebRequest.Create(url);
            var initialResult = (IAsyncResult)initialRequest.BeginGetResponse((IAsyncResult result) => {
                var request = (HttpWebRequest)result.AsyncState;
                var response = request.EndGetResponse(result);

                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream)) {
                    callback(JObject.Parse(reader.ReadToEnd()));
                }
            }, initialRequest);
        }

        public delegate void SearchDelegate(JArray result);

        public static void Search(GeoCoordinate location, string[] categories, SearchDelegate callback)
        {
            string url = String.Format(
                    FS_VENUE_SEARCH, 
                    location.Latitude, 
                    location.Longitude, 
                    string.Join(",", categories));
            GetRequest(url, (JObject response) => {
                callback((JArray)response["response"]["venues"]);
            });
        }

        public delegate void VenueDelegate(JObject venue);
        public static void Venue(string id, VenueDelegate callback) {
            string url = String.Format(FS_VENUE, id);
            GetRequest(url, (JObject response) => {
                callback((JObject)response["response"]["venue"]);
            });
        }

    }

}
