using System;
using Newtonsoft.Json;

namespace GolfCourseApp
{
    public class Tees
    {
        [JsonProperty("teeBox1")]
        public TeeBox TeeBox1 { get; set; }
    }

}

