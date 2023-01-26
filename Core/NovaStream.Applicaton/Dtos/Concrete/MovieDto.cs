namespace NovaStream.Application.Dtos.Concrete;

public record MovieDto : BaseVideoDto
{
    public MovieDto()
    {
        IsSerial = false;
    }
}

public record MovieSearchDto : MovieDto { }

public record MovieShortDetailsDto : VideoShortDetaislDto
{
    public MovieShortDetailsDto()
    {
        IsSerial = false;
    }
}

public record MovieDetailsDto : VideoDetailsDto
{
    public string VideoName { get; set; }
    public string VideoDescription { get; set; }
    public string VideoImageUrl { get; set; }
    public string VideoUrl { get; set; }
}
