namespace NovaStream.API.Controllers;

[ApiController, Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IUserManager _userManager;
    private readonly ITokenGeneratorService _tokenGeneratorService;


    public AccountController(IUserManager userManager, ITokenGeneratorService tokenGeneratorService)
    {
        _userManager = userManager;
        _tokenGeneratorService = tokenGeneratorService;
    }


    [HttpPost("[Action]")]
    public async Task<IActionResult> SignIn([FromBody] SignInUserDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = await _userManager.FindUserByEmailAsync(dto.Email);

        if (user is not null && await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            var token = await _tokenGeneratorService.GenerateTokenAsync(user);

            return Ok(token);
        }

        ModelState.AddModelError("Invalid", "Invalid email or password!");

        return Unauthorized(ModelState);
    }

    [HttpPost("[Action]")]
    public async Task<IActionResult> SignUp([FromBody] SignUpUserDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = new User() { Email = dto.Email };

        if (!await _userManager.ExistsAsync(user.Email))
        {
            var result = await _userManager.CreateUserAsync(user, dto.Password);

            if (result)
            {
                var token = await _tokenGeneratorService.GenerateTokenAsync(user);

                return Ok(token);
            }
        }
        else ModelState.AddModelError("Exists", "User with this email already exists!");

        return BadRequest(ModelState);
    }
}
