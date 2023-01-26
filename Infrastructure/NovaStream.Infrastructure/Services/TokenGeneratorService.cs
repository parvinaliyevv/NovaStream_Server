namespace NovaStream.Infrastructure.Services;

public class TokenGeneratorService : ITokenGeneratorService
{
    private readonly JsonWebTokenOptions _tokenOptions;


    public TokenGeneratorService(IOptions<JsonWebTokenOptions> tokenOptions)
    {
        _tokenOptions = tokenOptions.Value;
    }


    public string GenerateAuthorizeToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _tokenOptions.Issuer,
            audience: _tokenOptions.Audience,
            claims: claims,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public async Task<string> GenerateAuthorizeTokenAsync(User user)
        => await Task.Factory.StartNew(() => GenerateAuthorizeToken(user));
}
