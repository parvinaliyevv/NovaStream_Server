namespace NovaStream.Domain.Entities.Concrete.Adapters;

public class MovieCategory
{
    public string? MovieName { get; set; }
    public Movie? Movie { get; set; }
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
}
