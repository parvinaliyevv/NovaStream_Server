namespace NovaStream.Infrastructure.Services;

public class TokenGeneratorService : ITokenGeneratorService
{
    private readonly JsonWebTokenOptions _tokenOptions;


    public TokenGeneratorService(IOptions<JsonWebTokenOptions> tokenOptions)
    {
        _tokenOptions = tokenOptions.Value;
    }


    public string GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new Claim[]
        {
            new Claim(ClaimTypes.Email, user.Email)
        };

        var token = new JwtSecurityToken(
            issuer: _tokenOptions.Issuer,
            audience: _tokenOptions.Audience,
            claims: claims,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public async Task<string> GenerateTokenAsync(User user)
        => await Task.Factory.StartNew(() => GenerateToken(user));
}
