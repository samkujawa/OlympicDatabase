using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SQLite;

namespace GolfCourseApp
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _userDatabase;
        private SQLiteAsyncConnection _coursesDatabase;

        public DatabaseService()
        {
            string libFolder = FileSystem.AppDataDirectory;
            string userDbPath = Path.Combine(libFolder, "users.db");
            string coursesDbPath = Path.Combine(libFolder, "courses.db");


            Console.WriteLine("User Database Path: " + userDbPath);
            Console.WriteLine("Courses Database Path: " + coursesDbPath);

            _userDatabase = new SQLiteAsyncConnection(userDbPath);
            _coursesDatabase = new SQLiteAsyncConnection(coursesDbPath);


            _userDatabase.CreateTableAsync<User>().Wait(); // Create User table
            _coursesDatabase.CreateTableAsync<Courses>().Wait(); // Create Courses table

            //DeleteAllDataAsync().Wait();

        }

        public async Task DeleteAllDataAsync()
        {
            try
            {
                await _coursesDatabase.DropTableAsync<Courses>(); // Drop the Courses table
                await _coursesDatabase.CreateTableAsync<Courses>(); // Recreate the Courses table

                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting data: " + ex.Message);
            }
        }


        public static Task SetLoggedInUserAsync(string username)
        {
            Preferences.Set("IsLoggedIn", true);
            Preferences.Set("LoggedInUsername", username);
            return Task.CompletedTask;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _userDatabase.Table<User>().Where(u => u.Username == username).FirstOrDefaultAsync();
        }

        public async Task<int> SaveUserPreferenceAsync(UserPreference userPreference)
        {
            if (userPreference.Id != 0)
            {
                return await _userDatabase.UpdateAsync(userPreference);
            }
            else
            {
                return await _userDatabase.InsertAsync(userPreference);
            }
        }

        public async Task<User> GetUserPreferenceAsync()
        {
            return await _userDatabase.Table<User>().FirstOrDefaultAsync();
        }

        public async Task<bool> RegisterUserAsync(string username, string password)
        {
            if (password.Length < 2)
            {
                Console.WriteLine("Password is too short.");
                return false; // Password too short
            }

            Console.WriteLine("Attempting registration for username: " + username);

            string passwordHash = HashPasswordSecurely(password);

            var user = new User
            {
                Username = username,
                PasswordHash = passwordHash
            };

            int rowsInserted = await _userDatabase.InsertAsync(user);

            if (rowsInserted > 0)
            {
                Console.WriteLine("Registration successful.");
                return true;
            }
            else
            {
                Console.WriteLine("Registration failed.");
                return false;
            }
        }

        private string HashPasswordSecurely(string password)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
            return hashedPassword;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var user = await _userDatabase.Table<User>().Where(u => u.Username == username).FirstOrDefaultAsync();

            if (user != null)
            {
                bool isPasswordValid = VerifyPassword(password, user.PasswordHash);

                if (isPasswordValid)
                {
                    return true; // Login successful
                }
            }

            // Login failed
            return false;
        }

        private bool VerifyPassword(string password, string storedPasswordHash)
        {
            try
            {
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, storedPasswordHash);
                return isPasswordValid;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error verifying password: " + ex.Message);
                return false;
            }
        }

        public async Task SaveFavoriteCoursesAsync(List<Courses> favoriteCourses)
        {
            try
            {
                foreach (var course in favoriteCourses)
                {
                    bool isAlreadyFavorite = await IsCourseInFavoritesAsync(course);

                    if (!isAlreadyFavorite)
                    {
                        course.IsFavorite = true;
                        await _coursesDatabase.InsertAsync(course);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving favorite courses: " + ex.Message);
            }
        }


        private async Task<bool> IsCourseInFavoritesAsync(Courses course)
        {
            try
            {
                var existingCourse = await _coursesDatabase.Table<Courses>().Where(c => c.Id == course.Id && c.IsFavorite).FirstOrDefaultAsync();
                return existingCourse != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error checking if the course is in favorites: " + ex.Message);
                return false;
            }
        }




        public async Task<List<Courses>> GetFavoriteCoursesAsync()
        {
            try
            {
                return await _coursesDatabase.Table<Courses>().Where(c => c.IsFavorite).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving favorite courses: " + ex.Message);
                return new List<Courses>();
            }
        }
    }
}
