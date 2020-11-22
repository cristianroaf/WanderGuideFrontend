using System;
using System.Collections.Generic;
using System.Text;

namespace WanderGuideFrontend.Models
{
    public class Guide
    {
        public string _id { get; set; }
        public bool published { get; set; }
        public double rating { get; set; }
        public int total_votes { get; set; }
        public string user_id { get; set; }
        public string creator_username { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string title { get; set; }
        public int __v { get; set; }

    }
}
