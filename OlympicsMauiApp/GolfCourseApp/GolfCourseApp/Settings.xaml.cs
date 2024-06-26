using Microsoft.Maui.Controls;
using Plugin.Maui.Audio;
using System;
using System.Threading.Tasks;

namespace GolfCourseApp
{
    public partial class Settings : ContentPage
    {
        private DatabaseService _databaseService;
        private string _username;
        private GolfCourseViewModel _viewModel;
        private IAudioManager _audioManager;




        public Settings()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            _audioManager = DependencyService.Get<IAudioManager>();

            _viewModel = new GolfCourseViewModel(_databaseService, _audioManager);
            BindingContext = _viewModel;
            LoadCurrentTheme();

            if (Preferences.Get("IsLoggedIn", false))
            {
                Task.Run(async () => await LoadUserProfileInfoAsync()); 
            }
            else
            {
                UsernameLabel.Text = "User not logged in";
            }
        }



        private void LoadCurrentTheme()
        {
            if (!Preferences.ContainsKey("AppTheme"))
            {
                Preferences.Set("AppTheme", "Light");
            }
            string currentTheme = Preferences.Get("AppTheme", "Light");
            int selectedIndex = ThemePicker.Items.IndexOf(currentTheme);
            ThemePicker.SelectedIndex = selectedIndex >= 0 ? selectedIndex : 0;
        }

        private void OnThemePickerSelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;
            Theme selectedTheme = (Theme)picker.SelectedItem;

            SettingsService.Instance.Theme = selectedTheme;
        }



        private void ApplyThemeChanges(string selectedTheme)
        {
            var mergedDictionaries = Application.Current.Resources.MergedDictionaries;

            foreach (var dict in mergedDictionaries.ToList())
            {
                if (dict is GolfCourseApp.Resources.Styles.DarkTheme || dict is GolfCourseApp.Resources.Styles.LightTheme)
                {
                    mergedDictionaries.Remove(dict);
                }
            }

            switch (selectedTheme)
            {
                case "Dark":
                    mergedDictionaries.Add(new GolfCourseApp.Resources.Styles.DarkTheme());
                    break;
                case "Light":
                    mergedDictionaries.Add(new GolfCourseApp.Resources.Styles.LightTheme());
                    break;
            }
        }



        private Task LoadUserProfileInfoAsync()
        {
            string username = Preferences.Get("LoggedInUsername", "");

            if (!string.IsNullOrWhiteSpace(username))
            {
                UsernameLabel.Text = "Username: " + username;
            }
            else
            {
                UsernameLabel.Text = "User not logged in";
            }

            return Task.CompletedTask;
        }
    }
}
