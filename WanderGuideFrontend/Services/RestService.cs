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
            baseuri = "http://localhost:3000";
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
            catch {
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
    }
}
