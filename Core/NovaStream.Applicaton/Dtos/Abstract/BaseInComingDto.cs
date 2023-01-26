namespace NovaStream.Application.Dtos.Abstract;

public abstract record BaseInComingDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string TrailerUrl { get; set; }
    public string Categories { get; set; }
    public DateTime OutDate { get; set; }
}
