namespace NovaStream.Application.Dtos.Concrete;

public record SerialDto : BaseVideoDto
{
    public SerialDto()
    {
        IsSerial = true;
    }
}

public record SerialSearchDto : SerialDto { }

public record SerialShortDetailsDto : VideoShortDetaislDto
{
    public SerialShortDetailsDto()
    {
        IsSerial = true;
    }
}

public record SerialViewDetailsDto : VideoDetailsDto
{
    public string ImageUrl { get; set; }
}

public record SerialDetailsDto : VideoDetailsDto
{
    public ICollection<EpisodeDto> Episodes { get; set; }
}
