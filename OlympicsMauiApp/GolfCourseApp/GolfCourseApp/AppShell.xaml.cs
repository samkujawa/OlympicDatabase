namespace GolfCourseApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
        InitializeComponent();
        Routing.RegisterRoute(nameof(GolfCourseDetailPage), typeof(GolfCourseDetailPage));

    }

  
}