namespace NovaStream.Application.Dtos.Concrete;

public class MovieDto : BaseVideoDto
{
    public MovieDto()
    {
        IsSerial = false;
    }
}

public class MovieDetailsDto : VideoDetailsDto
{
    public string VideoPath { get; set; }


    public MovieDetailsDto()
    {
        IsSerial = false;
    }
}
