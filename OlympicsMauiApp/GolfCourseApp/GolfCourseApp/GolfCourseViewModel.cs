using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Plugin.Maui.Audio;
using Newtonsoft.Json;

namespace GolfCourseApp
{
    public class GolfCourseViewModel : INotifyPropertyChanged
    {
        private GolfCourseService _golfCourseService;
        private ObservableCollection<Courses> _golfCourses;
        private IAudioPlayer _loginSoundPlayer;
        private readonly IAudioManager _audioManager;
        private string _searchQuery;
        private DatabaseService _databaseService;
        public ICommand LoginCommand { get; private set; }
        public ICommand RegisterCommand { get; private set; }
        public ICommand NavigateToRegistrationCommand
        {
            get; private set;
        }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }


        public GolfCourseViewModel(DatabaseService databaseService, IAudioManager audioManager)
        {
            LoginCommand = new Command(async () => await LoginAsync());
            RegisterCommand = new Command(async () => await RegisterAsync());
            NavigateToRegistrationCommand = new Command<string>(NavigateToRegistrationPage);
            _databaseService = databaseService;
            this._audioManager = audioManager;
            if (_audioManager == null)
            {
                Console.WriteLine("AudioManager is not initialized.");
            }
            GolfCourses = new ObservableCollection<Courses>();
            SearchCommand = new Command(PerformSearch);
            LoadUserPreferences();
        }

        private async void LoadUserPreferences()
        {
            var userPreference = await _databaseService.GetUserPreferenceAsync();
            if (userPreference != null)
            {
                //FavoriteCourseId = userPreference.FavoriteCourseId;
                //AppTheme = userPreference.AppTheme;

                FavoriteCourses = new ObservableCollection<Courses>(await _databaseService.GetFavoriteCoursesAsync());
            }
        }

    

        private async void SavePreferences()
        {
            var userPreference = new UserPreference
            {
                FavoriteCourseId = this.FavoriteCourseId,
                AppTheme = this.AppTheme
            };

            await _databaseService.SaveUserPreferenceAsync(userPreference);

            await _databaseService.SaveFavoriteCoursesAsync(FavoriteCourses.ToList());
        }


        private string _favoriteCourseId;
        public string FavoriteCourseId
        {
            get => _favoriteCourseId;
            set
            {
                _favoriteCourseId = value;
                OnPropertyChanged(nameof(FavoriteCourseId));
                SavePreferences();
            }
        }

        private string _appTheme;
        public string AppTheme
        {
            get => _appTheme;
            set
            {
                _appTheme = value;
                OnPropertyChanged(nameof(AppTheme));
                SavePreferences();
            }
        }
        private Theme _currentTheme;
        public Theme CurrentTheme
        {
            get { return _currentTheme; }
            set
            {
                if (_currentTheme != value)
                {
                    _currentTheme = value;
                    OnPropertyChanged(nameof(CurrentTheme));
                }
            }
        }


        private ObservableCollection<Courses> _favoriteCourses;
        public ObservableCollection<Courses> FavoriteCourses
        {
            get => _favoriteCourses;
            set
            {
                _favoriteCourses = value;
                OnPropertyChanged(nameof(FavoriteCourses));
            }
        }


        private Courses _selectedCourse;
        public Courses SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                if (_selectedCourse != value)
                {
                    _selectedCourse = value;
                    OnPropertyChanged(nameof(SelectedCourse));
                    if (value != null)
                        NavigateToDetails(value);
                }
            }
        }

        public ObservableCollection<Courses> GolfCourses
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
                if (_searchQuery != value)
                {
                    _searchQuery = value;
                    OnPropertyChanged(nameof(SearchQuery));
                }
            }
        }

        public ICommand SearchCommand { get; }

        public GolfCourseViewModel()
        {
            LoginCommand = new Command(async () => await LoginAsync());
            RegisterCommand = new Command(async () => await RegisterAsync());
            NavigateToRegistrationCommand = new Command<string>(NavigateToRegistrationPage);
            _golfCourseService = new GolfCourseService();
            GolfCourses = new ObservableCollection<Courses>();
            SearchCommand = new Command(PerformSearch);
            _databaseService = new DatabaseService();
            Console.WriteLine("DatabaseService constructor called.");
        }

        private async void NavigateToRegistrationPage(object obj)
        {
            await Shell.Current.GoToAsync("//register");

        }

        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                return;
            }

            bool loginSuccess = await _databaseService.LoginAsync(Username, Password);

            if (loginSuccess)
            {
                try
                {
                    if (_loginSoundPlayer == null)
                    {
                        _loginSoundPlayer = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("success.mp3"));
                    }
                    _loginSoundPlayer.Play();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error playing sound: {ex.Message}");
                };

                await Shell.Current.GoToAsync($"//home");
            }
            else
            {
                // Handle login failure
                await App.Current.MainPage.DisplayAlert("Login Failed", "Invalid username or password", "OK");
            }
        }









        private async Task RegisterAsync()
        {
            bool registrationSuccess = await _databaseService.RegisterUserAsync(Username, Password);

            if (registrationSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Registration Successful", "You have successfully registered.", "OK");


                await Shell.Current.GoToAsync($"//home");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Registration Failed", "Registration failed. Please try again.", "OK");
            }
        }

        public async void PerformSearch()
        {
            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                try
                {
                    var courses = await _golfCourseService.GetGolfCoursesAsync(SearchQuery);
                    GolfCourses.Clear();

                    foreach (var course in courses)
                    {
                        GolfCourses.Add(new Courses()
                        {
                            courseId = course.courseId,
                            Name = course.Name,
                            Phone = course.Phone,
                            Website = course.Website,
                            Address = course.Address,
                            City = course.City,
                            State = course.State,
                            Zip = course.Zip,
                            Country = course.Country,
                            Coordinates = course.Coordinates,
                            Holes = course.Holes,
                            LengthFormat = course.LengthFormat,
                            GreenGrass = course.GreenGrass,
                            FairwayGrass = course.FairwayGrass,
                            Scorecard = course.Scorecard, // Assuming 'Scorecard' is the correct property name
                            TeeBoxes = course.TeeBoxes  // Assuming 'TeeBoxes' is the correct property name
                        });
                    }
                }
                catch (Exception ex)
                {
                    // Handle API call errors
                    Console.WriteLine("Error fetching golf courses: " + ex.Message);
                }
            }
        }

        private async void NavigateToDetails(Courses course)
        {
            try
            {
                var courseJson = JsonConvert.SerializeObject(course);
                var route = $"{nameof(GolfCourseDetailPage)}?course={Uri.EscapeDataString(courseJson)}";
                await Shell.Current.GoToAsync(route);
            }
            catch (JsonException ex)
            {
                // Handle JSON serialization error
                Console.WriteLine($"JSON Serialization Error: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
