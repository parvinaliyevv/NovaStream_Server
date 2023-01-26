namespace NovaStream.Application.Dtos;

public abstract class BaseMovieDto
{
    public string Name { get; set; }

    public string ImagePath { get; set; }
}

public class MovieDto : BaseMovieDto { }

public class DetailsMovieDto : BaseMovieDto
{
    public int Year { get; set; }
    public string Description { get; set; }
    public string VideoPath { get; set; }
}