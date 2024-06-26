using Xamarin.Forms;

namespace FinalProject;

public partial class GolfCourse : Microsoft.Maui.Controls.ContentPage
{
	public GolfCourse()
	{
		InitializeComponent();
        BindingContext = new GolfCourseViewModel();
    }
}
