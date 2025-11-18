using CineMate.Models;

namespace CineMate.Tmdb
{
    public static class TmdbExtensions
    {
        private const string ImageBaseUrl = "https://image.tmdb.org/t/p/w500";
        
        public static TmdbMovie ToTmdbMovie(this TMDbLib.Objects.Movies.Movie tmdbMovie)
        {
            return new TmdbMovie
            {
                Title = tmdbMovie.Title ?? "Unknown",
                PosterPath = string.IsNullOrEmpty(tmdbMovie.PosterPath)
                    ? string.Empty
                    : $"{ImageBaseUrl}{tmdbMovie.PosterPath}",
                ImdbId = tmdbMovie.ImdbId ?? string.Empty,
                Overview = tmdbMovie.Overview ?? string.Empty,
                ReleaseDate = tmdbMovie.ReleaseDate.HasValue
                    ? DateOnly.FromDateTime(tmdbMovie.ReleaseDate.Value)
                    : DateOnly.MinValue,
                Tagline = tmdbMovie.Tagline ?? string.Empty,
                VoteAverage = tmdbMovie.VoteAverage,
                Genres = tmdbMovie.Genres?.Select(g => g.Name).ToList() ?? []
            };
        }
    }
}
