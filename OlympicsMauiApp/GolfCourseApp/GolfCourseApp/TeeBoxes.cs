using System;
using Newtonsoft.Json;

namespace GolfCourseApp
{
    public class TeeBoxes
    {
        [JsonProperty("tee")]
        public string Tee { get; set; }

        [JsonProperty("slope")]
        public int Slope { get; set; }

        [JsonProperty("handicap")]
        public double Handicap { get; set; }
    }

}
