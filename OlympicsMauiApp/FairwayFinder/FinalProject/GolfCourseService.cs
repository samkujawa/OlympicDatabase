using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace FinalProject;

public class GolfCourseService
{
    private readonly HttpClient _client;

    public GolfCourseService()
    {
        _client = new HttpClient();
    }

    public async Task<List<GolfCourse>> GetGolfCoursesAsync(string searchQuery)
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
            return JsonConvert.DeserializeObject<List<GolfCourse>>(responseString);
        }
    }

}


