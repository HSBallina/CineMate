using CineMate.Configuration;
using CineMate.Models;
using TMDbLib.Client;

namespace CineMate.Tmdb
{
    public class Tmdb(ApiSettings settings) : ITmdb
    {
        public async Task<TmdbMovie> GetMovie(GptMovie gptMovie)
        {
            TMDbClient tMDbClient = new(settings.TmdbApiKey);

            var tmdbResults = await tMDbClient.SearchMovieAsync(gptMovie.Title, primaryReleaseYear: gptMovie.Year);

            return new TmdbMovie
            {
                Title = tmdbResults.Results.First().Title,
                PosterPath = tmdbResults.Results.First().PosterPath,
                VoteAverage = tmdbResults.Results.First().VoteAverage
            };
        }
    }
}