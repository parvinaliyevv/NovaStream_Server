namespace NovaStream.Application.Dtos.Concrete;

public class MovieDto : VideoDto
{
    public MovieDto()
    {
        IsSerial = false;
    }
}

public class MovieDetailsDto : VideoDetailsDto
{
    public string VideoName { get; set; }
    public string VideoDescription { get; set; }
    public string VideoImageUrl { get; set; }
    public string VideoUrl { get; set; }
}

public class MovieSearchDto : VideoSearchDto
{
    public MovieSearchDto()
    {
        IsSerial = false;
    }
}
