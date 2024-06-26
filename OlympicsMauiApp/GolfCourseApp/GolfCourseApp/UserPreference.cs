using System;
using SQLite;
namespace GolfCourseApp
{
    public class UserPreference
    {
        public int Id { get; set; }
        public string FavoriteCourseId { get; set; }
        public string AppTheme { get; set; }

        public UserPreference()
        {
        }

    }
}