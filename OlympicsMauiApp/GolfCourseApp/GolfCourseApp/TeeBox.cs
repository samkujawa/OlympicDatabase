using System;
using Newtonsoft.Json;
namespace GolfCourseApp
{
    public class TeeBox
    {
        [JsonProperty("color")]
        public string Color { get; set; }
        [JsonProperty("yards")]
        public int Yards { get; set; }
        public TeeBox()
        {
        }
    }
}