using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WanderGuideFrontend.CustomClasses;
using WanderGuideFrontend.Models;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace WanderGuideFrontend.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        public CustomMap Map { get; set; }
        public MapViewModel()
        {
            Map = new CustomMap
            {
                MapType = MapType.Street,
                HasZoomEnabled = true,
                IsShowingUser = true,
                CustomPins = new List<CustomPin>()
            };
        }
        public async void OnAppearing()
        {
            Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(0, 0), Distance.FromMeters(10000000)));
            Location loc = await Geolocation.GetLocationAsync();
            Map.CustomPins.Clear();
            Map.Pins.Clear();

            Services.RestService connection = new Services.RestService();
            (List<Guide> guides, int statuscode) = await connection.GetPublishedGuides();
            if (statuscode == 200)
            {
                //Map.CustomPins = new List<CustomPin>();
                foreach (Guide guide in guides)
                {
                    CustomPin location = new CustomPin
                    {
                        Type = PinType.Place,
                        Position = new Position(guide.latitude, guide.longitude),
                        Label = guide.title, //Titulo label
                        Address = guide.creator_username, //Persona snippet
                        Name = Math.Round(guide.rating, 1).ToString(), //Rating
                        Url = guide._id
                    };
                    Map.CustomPins.Add(location);
                    Map.Pins.Add(location);
                }
            }
            await Task.Delay(2000);
            Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(loc.Latitude, loc.Longitude), Distance.FromMeters(1500000)));
        }
    }
}