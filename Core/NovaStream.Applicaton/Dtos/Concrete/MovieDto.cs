namespace NovaStream.Application.Dtos.Concrete;

public record MovieDto : BaseVideoDto
{
    public MovieDto()
    {
        IsSerial = false;
    }
}

public record MovieSearchDto : MovieDto { }

public record MovieDetailsDto : BaseVideoDetailsDto
{
    public string VideoName { get; set; }
    public string VideoDescription { get; set; }
    public string VideoImageUrl { get; set; }
    public string VideoUrl { get; set; }
    public string VideoLength { get; set; }
    public string TrailerUrl { get; set; }

    public DirectorDto Director { get; set; }
    public ICollection<ActorDto> Actors { get; set; }
}

public record MovieViewDetailsDto : BaseVideoDetailsDto
{
    public string ImageUrl { get; set; }
}
