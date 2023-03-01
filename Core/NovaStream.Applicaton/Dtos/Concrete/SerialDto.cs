namespace NovaStream.Application.Dtos.Concrete;

public record SerialDto : BaseVideoDto
{
    public SerialDto()
    {
        IsSerial = true;
    }
}

public record SerialSearchDto : SerialDto { }

public record SerialDetailsDto : BaseVideoDetailsDto
{
    public string TrailerUrl { get; set; }
    public int SeasonCount { get; set; }

    public DirectorDto Director { get; set; }
    public ICollection<ActorDto> Actors { get; set; }
    public ICollection<EpisodeDto> Episodes { get; set; }
}

public record SerialViewDetailsDto : BaseVideoDetailsDto
{
    public string ImageUrl { get; set; }
    public int SeasonCount { get; set; }
}
