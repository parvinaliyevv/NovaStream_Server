namespace NovaStream.Application.Dtos.Concrete;

public class SerialDto : VideoDto
{
    public SerialDto()
    {
        IsSerial = true;
    }
}

public class SerialDetailsDto : VideoDetailsDto
{
    public ICollection<EpisodeDto> Episodes { get; set; }
}

public class SerialSearchDto : VideoSearchDto
{
    public SerialSearchDto()
    {
        IsSerial = true;
    }
}
