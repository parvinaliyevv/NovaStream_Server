namespace NovaStream.Application.Dtos.Abstract;

public abstract record BaseEpisodeDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string VideoLength { get; set; }
}
