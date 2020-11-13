namespace WanderGuideFrontend.Models
{
    public class User
    {
        public string _id { get; set; }
        public bool Verified { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int __v { get; set; }
    }
}
