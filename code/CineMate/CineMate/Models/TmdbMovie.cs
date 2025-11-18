namespace CineMate.Models;

public class TmdbMovie
{
    public required string Title { get; set; }
    public required string PosterPath { get; set; }
    public required string ImdbId { get; set; }
    public required string Overview { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public required string Tagline { get; set; }
    public double VoteAverage { get; set; }
    public required List<string> Genres { get; set; }
}