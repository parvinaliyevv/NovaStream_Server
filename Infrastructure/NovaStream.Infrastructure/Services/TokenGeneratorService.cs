namespace NovaStream.Infrastructure.Services;

public class TokenGeneratorService : ITokenGeneratorService
{
    private readonly IConfiguration _configuration;


    public TokenGeneratorService(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public string GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new Claim[]
        {
            new Claim(ClaimTypes.Email, user.Email)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: claims,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> GenerateTokenAsync(User user) 
        => await Task.Factory.StartNew(() => GenerateToken(user));
}
