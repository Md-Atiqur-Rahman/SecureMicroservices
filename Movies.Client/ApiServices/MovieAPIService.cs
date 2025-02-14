using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Movies.Client.Models;
using Newtonsoft.Json;

namespace Movies.Client.ApiServices;

public class MovieAPIService : IMovieAPIService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public MovieAPIService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public Task<bool> CreateMovie(Movie movie)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteMovie(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Movie> GetMovie(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Movie>> GetMovies()
    {
        //way 1
        //HttpClientFactory ব্যবহার করে কোডকে সাধারণ (generic) করা।
        var httpClient = _httpClientFactory.CreateClient("MovieApiClient");
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/movies/");

        var response  = await httpClient.SendAsync(request,HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        List<Movie> result = JsonConvert.DeserializeObject<List<Movie>>(content);
        return result;

        // Way 2
        // 1- Get token form Idnetity server 
        // 1. retrive our api credentials ,these must be registered on identity server
        //var apiCredintials = new ClientCredentialsTokenRequest
        //{
        //    Address = "https://localhost:5005/connect/token",
        //    ClientId = "movieClient",
        //    ClientSecret ="secret",

        //    //this is the scope of our protected Api requires
        //    Scope = "movieAPI"

        //};

        //// create a new HTTPClient to talk our identity server 
        //var client = new HttpClient();

        //var discovery = await client.GetDiscoveryDocumentAsync("https://localhost:5005");
        //if (discovery.IsError)
        //{
        //    return null;
        //}

        //// Authenticates and get a token from Identity server

        //var tokenResponse = await client.RequestClientCredentialsTokenAsync(apiCredintials);
        //if (tokenResponse.IsError)
        //{
        //    return null;
        //}

        //// 2- Send request to Protected Api
        //var apiClient = new HttpClient();

        //// 3- set the token in the request Authorization : Bearer <token)
        //apiClient.SetBearerToken(tokenResponse.AccessToken);

        //// 4- send a request to our Protected API
        //var response = await apiClient.GetAsync("https://localhost:5001/api/movies");
        //response.EnsureSuccessStatusCode();

        //// 5- Deserilize object to movieList
        //var content = await response.Content.ReadAsStringAsync();
        //List<Movie> result = JsonConvert.DeserializeObject<List<Movie>>(content);
        //return result;

    }

    public Task<bool> UpdateMovie(int id, Movie movie)
    {
        throw new NotImplementedException();
    }
}
