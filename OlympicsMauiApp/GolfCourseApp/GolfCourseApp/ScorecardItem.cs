using System;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GolfCourseApp
{

    [Table("ScorecardItems")]
    public class ScorecardItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [JsonProperty("Hole")]
        public int Hole { get; set; }

        [JsonProperty("Par")]
        public int Par { get; set; }

        [JsonProperty("tees")]
        public Tees Tees { get; set; }

        [JsonProperty("Handicap")]
        public int Handicap { get; set; }

        [ForeignKey(typeof(Courses))]
        public int CourseId { get; set; }
    }

}