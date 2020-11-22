using System;
using System.Collections.Generic;
using System.Text;

namespace WanderGuideFrontend.Models
{
    public class Stop
    {
        public string _id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string guide_id { get; set; }
        public int __v { get; set; }

        public override string ToString()
        {
            return title;
        }
    }

}
