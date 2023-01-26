namespace NovaStream.Domain.Entities.Abstract;

public abstract class Video
{
    public string Name { get; set; }
    public int Year { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
}
