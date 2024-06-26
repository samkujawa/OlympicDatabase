using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GolfCourseApp
{
    public class SettingsService : INotifyPropertyChanged
    {
        private static SettingsService _instance;
        public static SettingsService Instance => _instance ??= new SettingsService();

        private SettingsService()
        {
            Theme = LoadSelectedTheme();
        }

        private Theme _theme;
        public Theme Theme
        {
            get => _theme;
            set
            {
                if (_theme == value) return;
                _theme = value;
                OnPropertyChanged();
                SaveSelectedTheme(value);
            }
        }

        private Theme LoadSelectedTheme()
        {
            return Theme.System;
        }

        private void SaveSelectedTheme(Theme theme)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
