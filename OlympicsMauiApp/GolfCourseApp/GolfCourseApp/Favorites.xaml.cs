using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace GolfCourseApp;

public partial class Favorites : ContentPage
{
    public ObservableCollection<Courses> FavoriteCourses { get; set; }

    public Favorites()
    {
        InitializeComponent();
        FavoriteCourses = new ObservableCollection<Courses>(); 

        BindingContext = this;
        LoadFavoriteCourses();
    }

    private async void LoadFavoriteCourses()
    {
        var databaseService = new DatabaseService(); 
        var favoriteCourses = await databaseService.GetFavoriteCoursesAsync();
        Console.WriteLine($"Number of favorite courses: {favoriteCourses.Count}");


        foreach (var course in favoriteCourses)
        {
            FavoriteCourses.Add(course);
        }
    }

    private void OnCourseSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Courses selectedCourse)
        {
            
            var courseJson = JsonConvert.SerializeObject(selectedCourse);
            var route = $"{nameof(GolfCourseDetailPage)}?course={Uri.EscapeDataString(courseJson)}";
            Shell.Current.GoToAsync(route);
        }
    }

}
