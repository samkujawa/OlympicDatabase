using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GolfCourseApp;

public partial class GolfCourse : ContentPage
{
	public GolfCourse()
	{
		InitializeComponent();
        BindingContext = new GolfCourseViewModel();

    }

    protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
    {
        if (!Equals(field, newValue))
        {
            field = newValue;
            OnPropertyChanged(propertyName);
            return true;
        }

        return false;
    }

    private System.Collections.IEnumerable scorecard;

    public System.Collections.IEnumerable Scorecard { get => scorecard; set => SetProperty(ref scorecard, value); }
}
