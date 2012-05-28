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
using System.Device.Location;

namespace GTL
{
    public class LocationController
    {
        public delegate void LocationDelegate(GeoCoordinate coord);
        public static void getCurrent(LocationDelegate callback)
        {
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            watcher.MovementThreshold = 20; // use MovementThreshold to ignore noise in the signal
            //watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(
                (object sender, GeoPositionChangedEventArgs<GeoCoordinate> e) =>
                {
                    System.Diagnostics.Debug.WriteLine(e.Position.Location);
                    watcher.Stop();
                    callback(e.Position.Location);
                });
            watcher.Start();
        }
    
    }
}
