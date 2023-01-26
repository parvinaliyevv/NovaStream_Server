namespace NovaStream.Application.Dtos.Abstract;

public abstract class BaseVideoDto
{
    public string Name { get; set; }
    public string ImagePath { get; set; }
    public bool IsSerial { get; set; }
}

public abstract class VideoDetailsDto: BaseVideoDto
{
    public int Year { get; set; }
    public string Description { get; set; }
}
