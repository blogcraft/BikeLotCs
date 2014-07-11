using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BikeLot.Resources;

using Microsoft.Phone.Maps.Controls;
using System.Device.Location; // Provides the GeoCoordinate class.
using Windows.Devices.Geolocation; //Provides the Geocoordinate class.
using System.Windows.Media;
using System.Windows.Shapes;

using System.Threading.Tasks;

using Newtonsoft.Json;
using System.Net.Http;

namespace BikeLot
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            ShowMyLocationOnTheMap();
            GetLocations();
        }

        GeoCoordinate myGeoCoordinate;
        private async void ShowMyLocationOnTheMap()
        {
            // Get my current location.
            Geolocator myGeolocator = new Geolocator();
            myGeolocator.DesiredAccuracy = PositionAccuracy.High;
            Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync();
            Geocoordinate myGeocoordinate = myGeoposition.Coordinate;
            this.myGeoCoordinate = CoordinateConverter.ConvertGeocoordinate(myGeocoordinate);

            // Make my current location the center of the Map.
            this.Mapa.Center = myGeoCoordinate;
            this.Mapa.ZoomLevel = 13;

            // Create a small circle to mark the current location.
            Ellipse myCircle = new Ellipse();
            myCircle.Fill = new SolidColorBrush(Colors.Blue);
            myCircle.Height = 20;
            myCircle.Width = 20;
            myCircle.Opacity = 50;

            // Create a MapOverlay to contain the circle.
            MapOverlay myLocationOverlay = new MapOverlay();
            myLocationOverlay.Content = myCircle;
            myLocationOverlay.PositionOrigin = new Point(0.5, 0.5);
            myLocationOverlay.GeoCoordinate = myGeoCoordinate;

            // Create a MapLayer to contain the MapOverlay.
            MapLayer myLocationLayer = new MapLayer();
            myLocationLayer.Add(myLocationOverlay);

            // Add the MapLayer to the Map.
            Mapa.Layers.Add(myLocationLayer);
        }

        private async void GetLocations()
        {
            Uri theUri = new Uri("http://bikelot.herokuapp.com/locations");

            HttpClient aClient = new HttpClient();

            aClient.DefaultRequestHeaders.Host = theUri.Host;

            // then pass it as the first argument to the StringContent constructor 
            //StringContent theContent = new StringContent("location", System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");


            //GeoCoordinate topLeft = Mapa.ConvertViewportPointToGeoCoordinate(new Point(0, 0));
            //GeoCoordinate bottomRight =
            //Mapa.ConvertViewportPointToGeoCoordinate(new Point(Mapa.Width, Mapa.Height));

            //return LocationRectangle.CreateBoundingRectangle(new[] { topLeft, bottomRight });

            //Post the data 
            HttpResponseMessage aResponse = await aClient.GetAsync(theUri);
            string responseBody = await aResponse.Content.ReadAsStringAsync();

            List<Coordenate> Coordenadas = JsonConvert.DeserializeObject<List<Coordenate>>(responseBody);
                //new JavaScriptSerializer().Deserialize<Friends>(result);
            foreach (Coordenate coord in Coordenadas)
            {

            }
        }

        void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            Dispatcher.BeginInvoke(() =>
            {
                ShowMyLocationOnTheMap();
            });
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Coordenate myCoordenada = new Coordenate();
            myCoordenada.latitude = myGeoCoordinate.Latitude;
            myCoordenada.longitude = myGeoCoordinate.Longitude;

            string serial;

            serial = JsonConvert.SerializeObject(myCoordenada);

            Uri theUri = new Uri("http://bikelot.herokuapp.com/locations");

            //Create an Http client and set the headers we want 
            HttpClient aClient = new HttpClient();

            //aClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //aClient.DefaultRequestHeaders.Add("X-ZUMO-INSTALLATION-ID", "8bc6aea9-864a-44fc-9b4b-87ec64e123bd");
            //aClient.DefaultRequestHeaders.Add("X-ZUMO-APPLICATION", "OabcWgaGVdIXpqwbMTdBQcxyrOpeXa20");
            aClient.DefaultRequestHeaders.Host = theUri.Host;

            // then pass it as the first argument to the StringContent constructor 
            StringContent theContent = new StringContent("location[latitude]=" + myGeoCoordinate.Latitude.ToString().Replace(",", ".") + "&location[longitude]=" + myGeoCoordinate.Longitude.ToString().Replace(",", ".") + "&location[spots]=" + 0, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
            
            //Post the data 
            HttpResponseMessage aResponse= await aClient.PostAsync(theUri, theContent);

            if (aResponse.IsSuccessStatusCode)
            {
                MessageBox.Show("Thanks! :D");
            }
            else
            {
                // show the response status code 
                String failureMsg = "HTTP Status: " + aResponse.StatusCode.ToString() + " - Reason: " + aResponse.ReasonPhrase;
                MessageBox.Show(failureMsg);
            } 
        }

    }
}