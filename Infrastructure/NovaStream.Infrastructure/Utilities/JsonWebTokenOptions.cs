namespace NovaStream.Infrastructure.Utilities;

public class JsonWebTokenOptions
{
    public string? Key { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
}
