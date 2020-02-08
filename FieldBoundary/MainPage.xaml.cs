using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Net.Http;

using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Diagnostics;

namespace FieldBoundary
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            polygon = new Polygon
             {
                 StrokeWidth = 4,
                 StrokeColor = Color.FromHex("#1BA1E2"),
                 FillColor = Color.FromHex("#881BA1E2")
             };

            Position initPos = new Position(51.06, 10.35);
            MapSpan initSpan = new MapSpan(initPos, 5, 5);
            map = new Map(initSpan)
            { 
                MapType = MapType.Street
            };
            
            Pin pin = new Pin
            {
                Label = "Field",
                Type = PinType.Place,
                Position = new Position(49.8737566, 8.6909503)
            };
            map.Pins.Add(pin);
            
            map.MapClicked += OnMapClicked;
            Content = map;
            InitializeComponent();
        }

        private Polygon polygon;
        private Map map;

        async void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            Debug.WriteLine($"MapClick: {e.Position.Latitude}, {e.Position.Longitude}");
            map.Pins.Clear();
            map.MapElements.Remove(polygon);
            polygon.Geopath.Clear();

            //string fieldName = "defaulf field name";
            double[][] arr= new double[0][];
            Feature feature = await App.BoundaryManager.DetectionsTaskAsync(e.Position);
            if(feature.id != 0)
            {
                Debug.WriteLine("feature");
                arr = feature.geometry.coordinates[0];
            }
            

            if (arr.Length > 0)
            {

                Debug.WriteLine("arr.length");
                string fieldName = await DisplayPromptAsync("New Field", "Enter the field name");

               
                Debug.WriteLine(@"\t arr {0}", JsonConvert.SerializeObject(arr));
                foreach (double[] item in arr)
                {
                    Debug.WriteLine(@"\t item {0}", JsonConvert.SerializeObject(item));
                    Position p = new Position(item[1], item[0]);
                    polygon.Geopath.Add(p);
                }

                if (fieldName.Length > 0)
                {
                    Pin pin = new Pin
                    {
                        Label = fieldName,
                        Type = PinType.Place,
                        Position = new Position(e.Position.Latitude, e.Position.Longitude)
                    };
                    map.Pins.Add(pin);
                    map.MapElements.Add(polygon);
                }


                Debug.WriteLine(@"\t polygon {0}", JsonConvert.SerializeObject(polygon));
                
            }
            else
            {
                Debug.WriteLine(@"\t arr length is: {0}", arr.Length);
                await DisplayAlert("Sorry:", "No Field Here!", "OK");
            }


            
        }

    }
}
