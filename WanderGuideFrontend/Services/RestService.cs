using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WanderGuideFrontend.Models;

namespace WanderGuideFrontend.Services
{
    public class RestService
    {
        private readonly HttpClient client;
        private readonly string baseuri;

        public RestService()
        {
            client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
            //baseuri = "http://localhost:3000";
            baseuri = "http://wander-guide.herokuapp.com";
        }
        public async Task<(User, int)> Login(string username, string password)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri(baseuri + "/login"),
                    Content = new StringContent("{\"username\":\"" + username + "\",\"password\":\"" + password + "\"}", Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Post
                };
                request.Headers.Add("Accept", "application/json");
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    User user = JsonConvert.DeserializeObject<User>(content);
                    return (user, 200);
                }
                else if (response.StatusCode == HttpStatusCode.Gone)
                {
                    return (null, 410); //Incorrect Password 
                }
                else if (response.StatusCode == HttpStatusCode.LengthRequired)
                {
                    return (null, 411); //Username does not exist
                }
                else if (response.StatusCode == HttpStatusCode.PreconditionFailed)
                {
                    return (null, 412); //User not verified
                }
            }
            catch (Exception e){
                Console.WriteLine(e.Message);
                return (null, 408); } //Request timed out
            return (null, 400); //Unknown error
        }
        public async Task<int> Register(string email, string username, string password)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri(baseuri + "/register"),
                    Content = new StringContent("{\"email\":\"" + email + "\",\"username\":\"" + username + "\",\"password\":\"" + password + "\"}", Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Post
                };
                request.Headers.Add("Accept", "application/json");
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return 200; //Verification email sent
                }
                else if (response.StatusCode == HttpStatusCode.Gone)
                {
                    return 410; //Provided email is not valid
                }
                else if (response.StatusCode == HttpStatusCode.LengthRequired)
                {
                    return 411; //Username does not exist
                }
            }
            catch { return 408; } //Request timed out
            return 400; //Unknown error
        }
        public async Task<int> PasswordRecovery(string email)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri(baseuri + "/user/recovery"),
                    Content = new StringContent("{\"email\":\"" + email + "\"}", Encoding.UTF8, "application/json")
                };
                request.Headers.Add("Accept", "application/json");
                request.Method = HttpMethod.Post;
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return 200; //Recovery email sent
                }
                else if (response.StatusCode == HttpStatusCode.LengthRequired)
                {
                    return 411; //Email is not correct
                }
            }
            catch { return 408; }
            return 400;
        }
        public async Task<(Profile, int)> GetProfile(string id)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(baseuri + "/profile/" + id),
                Method = HttpMethod.Get
            };
            request.Headers.Add("Accept", "application/json");
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                Profile profile = JsonConvert.DeserializeObject<Profile>(content);
                return (profile, 200);
            }
            return (null, 400);
        }
        public async Task<(ProfileImage, int)> GetProfileImage(string id)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(baseuri + "/profile/" + id + "/image"),
                Method = HttpMethod.Get
            };
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                ProfileImage profileImage = JsonConvert.DeserializeObject<ProfileImage>(content);
                return (profileImage, 200);
            }
            return (null, 400);
        }
        public async Task<int> UpdateProfile(string id, string name, string description, string age)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri(baseuri + "/profile/" + id),
                    Content = new StringContent("{\"name\":\"" + name + "\",\"description\":\"" + description + "\",\"age\":\"" + age + "\"}", Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Post
                };
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return 200; //Profile updated
                }
            }
            catch
            {
                return 408;
            }
            return 400;
        }
        public async Task<int> UpdateProfileImage(string id, byte[] buffer)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(baseuri + "/profile/" + id + "/image")
            };
            string imageAsString = Convert.ToBase64String(buffer);
            request.Content = new StringContent("{\"buffer\":\"" + imageAsString + "\"}", Encoding.UTF8, "application/json");
            request.Headers.Add("Accept", "application/json");
            request.Method = HttpMethod.Post;
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return 200;
            }
            return 400;
        }
        public async Task<(List<Guide>, int)> GetPublishedGuides()
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri(baseuri + "/guides/"),
                    Method = HttpMethod.Get
                };
                request.Headers.Add("Accept", "application/json");
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    List<Guide> guides = JsonConvert.DeserializeObject<List<Guide>>(content);
                    return (guides, 200);
                }
                else if (response.StatusCode == HttpStatusCode.Created)
                {
                    return (null, 201); //There are no guides
                }
                return (null, 400);
            }
            catch
            {
                return (null, 408);
            }

        }
        public async Task<(Guide, int)> GetPublishedGuide(string id)
        {

            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(baseuri + "/guides/" + id),
                Method = HttpMethod.Get
            };
            request.Headers.Add("Accept", "application/json");
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                Guide guide = JsonConvert.DeserializeObject<Guide>(content);
                return (guide, 200);
            }
            else if (response.StatusCode == HttpStatusCode.Created)
            {
                return (null, 201); //Guide does not exist
            }
            return (null, 400);
        }
        public async Task<(List<Stop>, int)> GetPublishedStops(string id)
        {

            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(baseuri + "/guides/" + id + "/stops/"),
                Method = HttpMethod.Get
            };
            request.Headers.Add("Accept", "application/json");
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                List<Stop> stops = JsonConvert.DeserializeObject<List<Stop>>(content);
                return (stops, 200);
            }
            else if (response.StatusCode == HttpStatusCode.Created)
            {
                return (null, 201); //User has no draft guide
            }
            return (null, 400);
        }
        public async Task<(Guide, int)> GetDraftGuide(string id)
        {

            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(baseuri + "/guides/" + id + "/draft/guide"),
                Method = HttpMethod.Get
            };
            request.Headers.Add("Accept", "application/json");
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                Guide guide = JsonConvert.DeserializeObject<Guide>(content);
                return (guide, 200);
            }
            else if (response.StatusCode == HttpStatusCode.Created)
            {
                return (null, 201); //User has no draft guide
            }
            return (null, 400);
        }
        public async Task<(List<Stop>, int)> GetDraftStops(string id)
        {

            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(baseuri + "/guides/" + id + "/draft/stops"),
                Method = HttpMethod.Get
            };
            request.Headers.Add("Accept", "application/json");
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                List<Stop> stops = JsonConvert.DeserializeObject<List<Stop>>(content);
                return (stops, 200);
            }
            else if (response.StatusCode == HttpStatusCode.Created)
            {
                return (null, 201); //User has no draft guide
            }
            return (null, 400);
        }
        public async Task<int> CreateStop(string id, string title, string description, double latitude, double longitude)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri(baseuri + "/guides/" + id + "/draft"),
                    Content = new StringContent("{\"title\":\"" + title + "\",\"description\":\"" + description + "\",\"latitude\":\"" + latitude + "\",\"longitude\":\"" + longitude + "\"}", Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Post
                };
                request.Headers.Add("Accept", "application/json");
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return 200; //Verification email sent
                }
            }
            catch { return 408; } //Request timed out
            return 400; //Unknown error

        }
        public async Task<int> DeleteStop(string id)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri(baseuri + "/guides/stop/" + id + "/delete"),
                    Content = new StringContent("{}", Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Post
                };
                request.Headers.Add("Accept", "application/json");
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return 200; //Verification email sent
                }
            }
            catch { return 408; } //Request timed out
            return 400; //Unknown error
        }
        public async Task<int> DeleteGuide(string user_id, string guide_id)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri(baseuri + "/guides/" + guide_id + "/delete"),
                    Content = new StringContent("{\"user_id\":\"" + user_id + "\"}", Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Post
                };
                request.Headers.Add("Accept", "application/json");
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return 200; //Verification email sent
                }
            }
            catch { return 408; } //Request timed out
            return 400; //Unknown error
        }
        public async Task<int> PublishDraftGuide(string id)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri(baseuri + "/guides/" + id + "/publish"),
                    Content = new StringContent("{}", Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Post
                };
                request.Headers.Add("Accept", "application/json");
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return 200; //Verification email sent
                }
            }
            catch { return 408; } //Request timed out
            return 400; //Unknown error
        }
        public async Task<int> UpdateDraftTitle(string id, string title)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri(baseuri + "/guides/" + id + "/draft/title"),
                    Content = new StringContent("{\"title\":\"" + title + "\"}", Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Post
                };
                request.Headers.Add("Accept", "application/json");
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return 200; //Title updated
                }
            }
            catch { return 408; } //Request timed out
            return 400; //Unknown error
        }
        public async Task<int> RateGuide(string user_id, string guide_id, double value)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri(baseuri + "/guides/rate"),
                    Content = new StringContent("{\"user_id\":\"" + user_id + "\",\"guide_id\":\"" + guide_id + "\",\"rate\":\"" + value + "\"}", Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Post
                };
                request.Headers.Add("Accept", "application/json");
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return 200; //Saved successfully
                }
                else if (response.StatusCode == HttpStatusCode.Created)
                {
                    return 201; //Guide does not exist
                }
                else if (response.StatusCode == HttpStatusCode.Accepted)
                {
                    return 202; //You can't rate your own guide
                }
                else if (response.StatusCode == HttpStatusCode.NonAuthoritativeInformation)
                {
                    return 203;//You already rates this guide
                }
            }
            catch { return 408; } //Request timed out
            return 400; //Unknown error

        }
    }
}
