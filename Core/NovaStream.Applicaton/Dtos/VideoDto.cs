namespace NovaStream.Application.Dtos;

public abstract class BaseVideoDto
{
    public string Name { get; set; }

    public string ImagePath { get; set; }
}

public class VideoDto : BaseVideoDto {}

public class DetailsVideoDto : BaseVideoDto 
{
    public int Year { get; set; }
    public string Description { get; set; }
    public string VideoPath { get; set; }
}