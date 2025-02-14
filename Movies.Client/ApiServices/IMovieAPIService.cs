using Movies.Client.Models;

namespace Movies.Client.ApiServices;

public interface IMovieAPIService
{
    Task<IEnumerable<Movie>> GetMovies();

    Task<Movie> GetMovie(int id);

    Task<bool> CreateMovie(Movie movie);

    Task<bool> UpdateMovie(int id, Movie movie);

    Task<bool> DeleteMovie(int id);
    Task<UserInfoViewModel> GetUserInfo();
}
