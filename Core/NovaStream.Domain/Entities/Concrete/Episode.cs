namespace NovaStream.Domain.Entities.Concrete;

public class Episode : Entity
{
    public string VideoPath { get; set; }
    public string EpisodeName { get; set; }
    public int SeasonId { get; set; }
    public Season Season { get; set; }
}
