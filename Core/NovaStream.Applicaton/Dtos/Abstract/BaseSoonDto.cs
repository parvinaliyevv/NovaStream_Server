namespace NovaStream.Application.Dtos.Abstract;

public abstract record BaseSoonDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string TrailerUrl { get; set; }
    public string TrailerImageUrl { get; set; }
    public string Categories { get; set; }
    public string Month { get; set; }
    public int Day { get; set; }
}
