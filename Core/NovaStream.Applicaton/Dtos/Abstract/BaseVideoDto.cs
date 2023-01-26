namespace NovaStream.Application.Dtos.Abstract;

public abstract class BaseVideoDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string TrailerUrl { get; set; }
    public int? SeasonCount { get; set; }
}

public abstract class VideoDto : BaseVideoDto
{
    public string Categories { get; set; }
    public bool? IsSerial { get; set; }

}

public abstract class VideoDetailsDto : BaseVideoDto
{
    public int Year { get; set; }
    public int Age { get; set; }
    public bool IsMarked { get; set; }
}

public abstract class VideoSearchDto
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public bool IsSerial { get; set; }
}
