using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Toast;
using System;
using System.IO;
using System.Threading.Tasks;
using WanderGuideFrontend.Models;
using Xamarin.Forms;

namespace WanderGuideFrontend.ViewModels
{
    internal class CreatorProfileViewModel : BaseViewModel
    {
        #region Attribute
        public string user_id;
        public ImageSource profile_image;
        public string username;
        public string name;
        public string description;
        public string age;
        public string member_since;
        public string button;

        public bool isEnabled = false;
        #endregion
        #region Properties
        public string DateTxt
        {
            get => member_since;
            set => SetValue(ref member_since, value);
        }
        public string DescriptionTxt
        {
            get => description;
            set => SetValue(ref description, value);
        }
        public string AgeTxt
        {
            get => age;
            set => SetValue(ref age, value);
        }
        public string ButtonTxt
        {
            get => button;
            set => SetValue(ref button, value);
        }
        public string NameTxt
        {
            get => name;
            set => SetValue(ref name, value);
        }
        public string UsernameTxt
        {
            get => username;
            set => SetValue(ref username, value);
        }

        public ImageSource ProfilePic
        {
            get => profile_image;
            set => SetValue(ref profile_image, value);
        }
        public bool IsEnabledTxt
        {
            get => isEnabled;
            set => SetValue(ref isEnabled, value);
        }
        #endregion
        public Command GetProfileInformationCommand { get; }
        public Command EditCommand { get; }
        public Command LogoutCommand { get; }
        public CreatorProfileViewModel(string user_id)
        {
            GetProfileInformationCommand = new Command(LoadProfileInformation);
            this.user_id = user_id;
        }

        public async void LoadProfileInformation()
        {
            ButtonTxt = "Edit";
            IsEnabledTxt = false;
            Services.RestService connection = new Services.RestService();
            (Profile p, int statuscode1) = await connection.GetProfile(user_id);
            (ProfileImage pi, int statuscode2) = await connection.GetProfileImage(user_id);
            if (statuscode1 == 200 && statuscode2 == 200)
            {
                ProfilePic = ImageSource.FromStream(() => new MemoryStream(pi.data));
                UsernameTxt = App.Username;
                NameTxt = p.name;
                DescriptionTxt = p.description;
                AgeTxt = p.age.ToString();
                DateTxt = p.creation_date.ToString();
            }
            else
            {
                CrossToastPopUp.Current.ShowToastMessage("An error ocurred loading profile");
            }
        }
    }
}

