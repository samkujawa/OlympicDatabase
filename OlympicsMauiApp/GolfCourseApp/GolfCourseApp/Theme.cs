using System;
namespace GolfCourseApp
{
    public sealed class Theme
    {
        public static Theme Dark = new(AppTheme.Dark, "Night Mode", ColorFromRgba(0, 0, 0, 255)); // Black
        public static Theme Light = new(AppTheme.Light, "Day Mode", ColorFromRgba(255, 255, 255, 255)); // White
        public static Theme System = new(AppTheme.Unspecified, "Follow System", ColorFromRgba(0, 0, 0, 0)); // Transparent

        public AppTheme AppTheme { get; }
        public string DisplayName { get; }
        public Color BackgroundColor { get; }

        private Theme(AppTheme theme, string displayName, Color backgroundColor)
        {
            AppTheme = theme;
            DisplayName = displayName;
            BackgroundColor = backgroundColor;
        }

        private static Color ColorFromRgba(int red, int green, int blue, int alpha)
        {
            return Color.FromRgba(red, green, blue, alpha);
        }
    }
}
