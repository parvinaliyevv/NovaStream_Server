namespace NovaStream.Application.Services;

public static class Manufacturer
{
    public static string ManufactureGenres(List<string> genres)
    {
        var builder = new StringBuilder();

        builder.Append($"{genres[0]} •");

        for (int i = 1; i < genres.Count - 1; i++) builder.Append($" {genres[i]} •");

        builder.Append($" {genres[genres.Count - 1]}");

        return builder.ToString();
    }

    public static string ManufactureTime(TimeSpan time)
    {
        var minutes = time.Minutes;

        return minutes > 60 ? $"{minutes / 60}h {minutes % 60}m" : $"{minutes % 60}m";
    }
}
