using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Toast;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WanderGuideFrontend.Models;
using WanderGuideFrontend.Services;
using Xamarin.Forms;

namespace WanderGuideFrontend.ViewModels
{
    internal class ProfileViewModel : BaseViewModel
    {
        #region Attribute
        public ImageSource profile_image;
        public string username;
        public string name;
        public string description;
        public string age;
        public string member_since;
        public string button;

        public bool isEnabled = true;
        public bool isEnabledPhoto = false;
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
        public bool IsEnabledPhotoTxt
        {
            get => isEnabledPhoto;
            set => SetValue(ref isEnabledPhoto, value);
        }
        #endregion
        public Command GetProfileInformationCommand { get; }
        public Command EditCommand { get; }
        public Command LogoutCommand { get; }
        public ProfileViewModel()
        {
            GetProfileInformationCommand = new Command(LoadProfileInformation);
            EditCommand = new Command(EditPressed);
            LogoutCommand = new Command(LogoutPressed);
        }

        public async void LoadProfileInformation()
        {
            ButtonTxt = "Edit";
            IsEnabledTxt = true;
            isEnabledPhoto = false;
            RestService connection = new Services.RestService();
            (Profile p, int statuscode1) = await connection.GetProfile(App.User_id);
            (ProfileImage pi, int statuscode2) = await connection.GetProfileImage(App.User_id);
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
        public async Task ProfileImageClicked()
        {

            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("Not supported", "Your device does not currently support this functionality", "Ok");
                return;
            }
            if (!CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("Not supported", "Your device does not currently support this functionality", "Ok");
                return;
            }
            //Select image
            try
            {
                string action = await Application.Current.MainPage.DisplayActionSheet("Choose the image from:", "Cancel", null, "Photo Roll", "Camera");
                if (action != "Cancel")
                {

                    if (action == "Photo Roll")
                    {
                        PickMediaOptions mediaOptions = new PickMediaOptions()
                        {
                            PhotoSize = PhotoSize.Custom,
                            CustomPhotoSize = 15,
                        };
                        MediaFile selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);
                        if (selectedImageFile != null)
                        {
                            //Upload to the server
                            MemoryStream memoryStream = new MemoryStream();
                            selectedImageFile.GetStream().CopyTo(memoryStream);
                            byte[] bytes = memoryStream.ToArray();

                            RestService connection = new Services.RestService();
                            int statuscode = await connection.UpdateProfileImage(App.User_id, bytes);
                            if (statuscode == 200)
                            {
                                //Change the profile pic
                                await Task.Delay(500);
                                ProfilePic = ImageSource.FromStream(() => selectedImageFile.GetStream());
                            }
                            else
                            {
                                throw new Exception();
                            }
                        }
                        else
                        {
                            CrossToastPopUp.Current.ShowToastMessage("No image selected");
                        }
                    }
                    else if (action == "Camera")
                    {
                        StoreCameraMediaOptions mediaOptions = new StoreCameraMediaOptions()
                        {
                            Directory = "Images",
                            Name = DateTime.Now + ".jpg",
                            PhotoSize = PhotoSize.Custom,
                            CustomPhotoSize = 10,
                            SaveToAlbum = true
                        };
                        MediaFile selectedImageFile = await CrossMedia.Current.TakePhotoAsync(mediaOptions);

                        if (selectedImageFile != null)
                        {
                            //Upload to the server
                            MemoryStream memoryStream = new MemoryStream();
                            selectedImageFile.GetStream().CopyTo(memoryStream);
                            byte[] bytes = memoryStream.ToArray();
                            RestService connection = new Services.RestService();
                            int statuscode = await connection.UpdateProfileImage(App.User_id, bytes);
                            if (statuscode == 200)
                            {
                                //Change the profile pic
                                await Task.Delay(500);
                                ProfilePic = ImageSource.FromStream(() => selectedImageFile.GetStream());
                            }
                            else
                            {
                                CrossToastPopUp.Current.ShowToastMessage("Not saved");
                            }
                        }
                        else
                        {
                            CrossToastPopUp.Current.ShowToastMessage("No image taken");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error:");
                Console.WriteLine(ex.Message);
                CrossToastPopUp.Current.ShowToastMessage("An error ocurred");
            }
        }
        public async void EditPressed()
        {
            if (ButtonTxt == "Edit")
            {
                ButtonTxt = "Save";
                IsEnabledTxt = false;
                isEnabledPhoto = true;
            }
            else
            {
                ButtonTxt = "Edit";
                IsEnabledTxt = true;
                isEnabledPhoto = false;
                RestService connection = new Services.RestService();
                int statuscode = await connection.UpdateProfile(App.User_id, NameTxt, DescriptionTxt, AgeTxt);
            }
            await Task.Delay(500);
        }
        public async void LogoutPressed()
        {
            App.Username = "";
            App.User_id = "";
            Settings.Autologin = false;
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
