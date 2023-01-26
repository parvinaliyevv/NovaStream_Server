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

public record SerialDetailsDto : VideoDetailsDto
{
    public ICollection<EpisodeDto> Episodes { get; set; }
}
