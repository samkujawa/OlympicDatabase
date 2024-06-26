using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace FinalProject;

public class GolfCourseViewModel : INotifyPropertyChanged
{
    private GolfCourseService _golfCourseService;
    private ObservableCollection<GolfCourse> _golfCourses;
    private string _searchQuery;

    public ObservableCollection<GolfCourse> GolfCourses
    {
        get => _golfCourses;
        set
        {
            _golfCourses = value;
            OnPropertyChanged(nameof(GolfCourses));
        }
    }

    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            _searchQuery = value;
            OnPropertyChanged(nameof(SearchQuery));
            PerformSearch();
        }
    }

    public ICommand SearchCommand { get; }

    public GolfCourseViewModel()
    {
        _golfCourseService = new GolfCourseService();
        GolfCourses = new ObservableCollection<GolfCourse>();
        SearchCommand = new Xamarin.Forms.Command(PerformSearch);
    }

    private async void PerformSearch()
    {
        if (!string.IsNullOrWhiteSpace(SearchQuery))
        {
            var courses = await _golfCourseService.GetGolfCoursesAsync(SearchQuery);
            GolfCourses.Clear();
            foreach (var course in courses)
            {
                GolfCourses.Add(course);
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
