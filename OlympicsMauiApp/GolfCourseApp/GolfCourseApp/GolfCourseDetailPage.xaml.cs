using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System;

namespace GolfCourseApp;

[QueryProperty(nameof(CourseJson), "course")]
public partial class GolfCourseDetailPage : ContentPage
{
    private GolfCourseViewModel viewModel;
    private DatabaseService _databaseService;
    private string courseJson;
    public string CourseJson
    {
        get => courseJson;
        set
        {
            courseJson = Uri.UnescapeDataString(value);
            try
            {
                var course = JsonConvert.DeserializeObject<Courses>(courseJson);
                if (course != null)
                {
                    BindingContext = course;
                }
                else
                {
                    Console.WriteLine("Deserialized course is null.");
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Deserialization Error: {ex.Message}");
            }
        }
    }


    public GolfCourseDetailPage()
    {
        InitializeComponent();
        viewModel = new GolfCourseViewModel();
        BindingContext = viewModel;
        _databaseService = new DatabaseService();

        try
        {
            viewModel.PerformSearch(); 
        }
        catch (InvalidCastException ex)
        {
            Console.WriteLine("InvalidCastException: " + ex.Message);

            Console.WriteLine("Exception stack trace: " + ex.StackTrace);

            Exception innerException = ex.InnerException;
            while (innerException != null)
            {
                Console.WriteLine("Inner Exception: " + innerException.Message);
                Console.WriteLine("Inner Exception stack trace: " + innerException.StackTrace);
                innerException = innerException.InnerException;
            }
        }
        UpdateFavoriteButtonUI();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is Courses course)
        {
            BindingContext = course;

        }
        UpdateFavoriteButtonUI();
    }



    private void UpdateFavoriteButtonUI()
    {
        if (BindingContext is Courses course)
        {
            FavoriteButton.Text = course.IsFavorite ? "Course added as a favorite" : "Add course as a favorite";
        }
    }



    private async void FavoriteButton_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is Courses course)
        {
            if (!course.IsFavorite)
            {
                course.IsFavorite = true;
                UpdateFavoriteButtonUI();

                await _databaseService.SaveFavoriteCoursesAsync(new List<Courses> { course });
            }
            else
            {
                await DisplayAlert("Favorite Course", "This course is already in your favorites.", "OK");
            }
        }
    }



}