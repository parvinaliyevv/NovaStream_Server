namespace NovaStream.Application.Dtos.Abstract;

public abstract record BaseProducerDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string About { get; set; }
    public string? ImageUrl { get; set; }
}
