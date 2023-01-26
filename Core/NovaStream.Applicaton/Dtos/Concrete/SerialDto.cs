namespace NovaStream.Application.Dtos.Concrete;

public class SerialDto : BaseVideoDto
{
    public SerialDto()
    {
        IsSerial = true;
    }
}

public class SerialDetailsDto : VideoDetailsDto
{
    public int SeasonCount { get; set; }
    public ICollection<EpisodeDto> Episodes { get; set; }


    public SerialDetailsDto()
    {
        IsSerial = true;
    }
}
