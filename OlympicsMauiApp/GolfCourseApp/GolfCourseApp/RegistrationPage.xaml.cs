using Microsoft.Maui.Controls;

namespace GolfCourseApp
{
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
            BindingContext = new GolfCourseViewModel();
        }
    }
}
