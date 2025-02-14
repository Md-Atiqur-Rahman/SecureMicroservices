using Movies.Client.Models;

namespace Movies.Client.ApiServices;

public class MovieAPIService : IMovieAPIService
{
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
        var movieList = new List<Movie>();
        movieList.Add(new Movie
        {
            Id=1,
            Genre="Comic",
            Title="asd",
            Rating="9.2",
            ImageUrl="image/resource",
            ReleaseDate = DateTime.Now,
            Owner="swn"
        });
        return await Task.FromResult(movieList);
    }

    public Task<bool> UpdateMovie(int id, Movie movie)
    {
        throw new NotImplementedException();
    }
}
