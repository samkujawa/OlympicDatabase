using System;
using System.ComponentModel;
using Newtonsoft.Json;
using SQLite;
using System.Collections.Generic;
using SQLiteNetExtensions.Attributes;

namespace GolfCourseApp;

[Table("Courses")]
public class Courses
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [JsonProperty("_id")]
    public string courseId { get; set; }
    public string Name { get; set; } = "Not Specified";
    public string Phone { get; set; } = "Not Specified";
    public string Website { get; set; } = "Not Specified";
    public string Address { get; set; } = "Not Specified";
    public string City { get; set; } = "Not Specified";
    public string State { get; set; } = "Not Specified";
    public string Zip { get; set; } = "Not Specified";
    public string Country { get; set; } = "Not Specified";
    public string Coordinates { get; set; } = "Not Specified";
    public string Holes { get; set; } = "Not Specified";
    public string LengthFormat { get; set; } = "Not Specified";
    public string GreenGrass { get; set; } = "Not Specified";
    public string FairwayGrass { get; set; } = "Not Specified";
    public string IsFavoriteText { get; set; }
    [JsonProperty("scorecard")]
    [OneToMany(CascadeOperations = CascadeOperation.All)]
    public List<ScorecardItem> Scorecard { get; set; }

    [JsonProperty("teeBoxes")]
    [OneToMany(CascadeOperations = CascadeOperation.All)]
    public List<TeeBoxes> TeeBoxes { get; set; }

    private bool isFavorite;
    public bool IsFavorite
    {
        get => isFavorite;
        set
        {
            if (isFavorite != value)
            {
                isFavorite = value;
                OnPropertyChanged(nameof(IsFavorite));
            }
        }
    }


    public event PropertyChangedEventHandler PropertyChanged;

    public Courses()
    {
        Scorecard = new List<ScorecardItem>();
        TeeBoxes = new List<TeeBoxes>();
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
