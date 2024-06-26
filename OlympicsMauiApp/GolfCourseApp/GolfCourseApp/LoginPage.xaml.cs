using Microsoft.Maui.Controls;
using System.Windows.Input;
using Plugin.Maui.Audio;

namespace GolfCourseApp
{
    public partial class LoginPage : ContentPage
    {
        private readonly IAudioManager audioManager;

        public LoginPage(IAudioManager audioManager)
        {
            InitializeComponent();
            this.audioManager = audioManager;
            if (audioManager == null)
            {
                Console.WriteLine("IAudioManager is null. Make sure it's registered correctly.");
            }
            else
            {
                Console.WriteLine("IAudioManager is not null. It's successfully initialized.");
            }
            BindingContext = new GolfCourseViewModel(new DatabaseService(), audioManager);
        }
    }
}
