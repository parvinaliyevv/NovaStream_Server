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
    public int Year { get; set; }
    public int Age { get; set; }
    public bool IsMarked { get; set; }
}
