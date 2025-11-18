using CineMate.Models;

namespace CineMate.Tmdb;

public interface ITmdb
{
    public Task<TmdbMovie> GetMovie(GptMovie gptMovie);
}
