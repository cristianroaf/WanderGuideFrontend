using System.Collections.Generic;
using WanderGuideFrontend.Models;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace WanderGuideFrontend.ViewModels
{

    public class GuideMapViewModel : BaseViewModel
    {
        public Xamarin.Forms.Maps.Map Map { get; set; }
        public string guide_id;
        public GuideMapViewModel(string guide_id)
        {
            Map = new Xamarin.Forms.Maps.Map
            {
                MapType = MapType.Street,
                HasZoomEnabled = true,
                IsShowingUser = true
            };
            this.guide_id = guide_id;
        }
        public async void OnAppearing()
        {
            Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(0, 0), Distance.FromMeters(10000000)));
            Location loc = await Geolocation.GetLocationAsync();
            Services.RestService connection = new Services.RestService();
            (List<Stop> stops, int statuscode1) = await connection.GetPublishedStops(guide_id);
            (Guide g, int statuscode2) = await connection.GetPublishedGuide(guide_id);
            if (statuscode1 == 200 && statuscode2 == 200)
            {
                Map.Pins.Clear();
                bool first = true;
                foreach (Stop stop in stops)
                {
                    if (first)
                    {
                        Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(stop.latitude, stop.longitude), Distance.FromMeters(5000)));
                        first = false;
                    }
                    Pin location = new Pin()
                    {
                        Type = PinType.Place,
                        Label = stop.title,
                        Address = stop.description,
                        Position = new Position(stop.latitude, stop.longitude),
                    };
                    Map.Pins.Add(location);
                }
            }
        }
    }
}