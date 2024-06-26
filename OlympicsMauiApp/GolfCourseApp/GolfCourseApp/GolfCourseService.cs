using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GolfCourseApp
{
    public class GolfCourseService
    {
        private readonly HttpClient _client;

        public GolfCourseService()
        {
            _client = new HttpClient();
        }

        public async Task<List<Courses>> GetGolfCoursesAsync(string searchQuery)
        {
            try
            {
                var requestUri = $"https://golf-course-api.p.rapidapi.com/search?name={Uri.EscapeDataString(searchQuery)}";
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(requestUri),
                    Headers =
            {
                { "X-RapidAPI-Key", "e286a2ccfamsh0d5279a72277e0ap14437ejsn0cebb08b0c61" },
                { "X-RapidAPI-Host", "golf-course-api.p.rapidapi.com" },
            },
                };

                using (var response = await _client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (responseString.StartsWith("["))
                    {
                        var courses = JsonConvert.DeserializeObject<List<Courses>>(responseString);

                        var modifiedCourses = new List<Courses>();

                        foreach (var course in courses)
                        {
                            var modifiedCourse = new Courses();

                            modifiedCourse.courseId = course.courseId;
                            modifiedCourse.Name = course.Name;
                            modifiedCourse.Address = course.Address;
                            modifiedCourse.City = course.City;
                            modifiedCourse.State = course.State;
                            modifiedCourse.Zip = course.Zip;
                            modifiedCourse.Phone = course.Phone;
                            modifiedCourse.Website = course.Website;
                            modifiedCourse.Country = course.Country;
                            modifiedCourse.Coordinates = course.Coordinates;
                            modifiedCourse.Holes = course.Holes;
                            modifiedCourse.LengthFormat = course.LengthFormat;
                            modifiedCourse.GreenGrass = course.GreenGrass;
                            modifiedCourse.FairwayGrass = course.FairwayGrass;
                            modifiedCourse.Scorecard = course.Scorecard;
                            modifiedCourse.TeeBoxes = course.TeeBoxes;

                            modifiedCourses.Add(modifiedCourse);
                        }

                        return modifiedCourses;
                    }
                    else
                    {
                        var course = JsonConvert.DeserializeObject<Courses>(responseString);

                        var modifiedCourse = new Courses();

                        modifiedCourse.courseId = course.courseId;
                        modifiedCourse.Name = course.Name;
                        modifiedCourse.Address = course.Address;
                        modifiedCourse.City = course.City;
                        modifiedCourse.State = course.State;
                        modifiedCourse.Zip = course.Zip;
                        modifiedCourse.Phone = course.Phone;
                        modifiedCourse.Website = course.Website;
                        modifiedCourse.Country = course.Country;
                        modifiedCourse.Coordinates = course.Coordinates;
                        modifiedCourse.Holes = course.Holes;
                        modifiedCourse.LengthFormat = course.LengthFormat;
                        modifiedCourse.GreenGrass = course.GreenGrass;
                        modifiedCourse.FairwayGrass = course.FairwayGrass;
                        modifiedCourse.Scorecard = course.Scorecard;
                        modifiedCourse.TeeBoxes = course.TeeBoxes;

                        return new List<Courses>() { modifiedCourse };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in API call: " + ex.Message);
                return new List<Courses>();
            }
        }
        
    }
}