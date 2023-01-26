namespace NovaStream.Application.Dtos.Abstract;

public abstract record BaseVideoDto
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public bool IsSerial { get; set; }
}

public abstract record BaseVideoDetailsDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string? TrailerUrl { get; set; }
    public int? SeasonCount { get; set; }
}

public abstract record VideoShortDetaislDto : BaseVideoDetailsDto
{
    public string Categories { get; set; }
    public string TrailerImageUrl { get; set; }
    public bool? IsSerial { get; set; }
}

public abstract record VideoDetailsDto : BaseVideoDetailsDto
{
    public int Year { get; set; }
    public int Age { get; set; }
    public bool IsMarked { get; set; }
}
