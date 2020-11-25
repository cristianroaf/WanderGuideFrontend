using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Enums;
using Rg.Plugins.Popup.Services;
using System.Collections.Generic;
using WanderGuideFrontend.Models;
using WanderGuideFrontend.Views;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace WanderGuideFrontend.ViewModels
{
    public class CreateStopViewModel : BaseViewModel
    {
        public Xamarin.Forms.Maps.Map Map { get; set; }
        public CreateStopViewModel()
        {
            Map = new Xamarin.Forms.Maps.Map();
            Map.MapClicked += Map_Clicked;
            Map.MapType = MapType.Street;
            Map.HasZoomEnabled = true;
            Map.IsShowingUser = true;

        }
        public async void OnAppearing()
        {
            Location loc = await Geolocation.GetLocationAsync();
            Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(loc.Latitude, loc.Longitude), Distance.FromMeters(5000)));
            Services.RestService connection = new Services.RestService();
            (List<Stop> stops, int statuscode) = await connection.GetDraftStops(App.User_id);
            if (statuscode == 200)
            {
                Map.Pins.Clear();
                foreach (Stop stop in stops)
                {
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
        public async void ReloadPins()
        {
            OnAppearing();
            return;
        }
        public void PopUpBackPressed()
        {
            Map.Pins.RemoveAt(Map.Pins.Count - 1);
        }
        private async void Map_Clicked(object sender, MapClickedEventArgs e)
        {

            Pin location = new Pin()
            {
                Type = PinType.Place,
                Label = "",
                Address = "",
                Position = new Position(e.Position.Latitude, e.Position.Longitude),
            };
            Map.Pins.Add(location);

            PopupView popupProperties = new PopupView(this, e.Position);
            ScaleAnimation scaleAnimation = new ScaleAnimation
            {
                PositionIn = MoveAnimationOptions.Right,
                PositionOut = MoveAnimationOptions.Left
            };

            popupProperties.Animation = scaleAnimation;

            await PopupNavigation.Instance.PushAsync(popupProperties, true);

        }


    }
}