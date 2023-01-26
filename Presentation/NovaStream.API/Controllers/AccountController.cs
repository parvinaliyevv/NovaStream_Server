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
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindUserByEmailAsync(dto.Email);

            if (user is not null && _userManager.CheckPassword(user, dto.Password))
            {
                var token = await _tokenGeneratorService.GenerateTokenAsync(user);
                var response = new { Email = dto.Email, PasswordLength = dto.Password.Length, Token = token };
                var json = JsonConvert.SerializeObject(response, Formatting.Indented);

                return Ok(json);
            }

            return Unauthorized();
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost("[Action]")]
    public async Task<IActionResult> SignUp([FromBody] SignUpUserDto dto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = dto.Adapt<User>();

            if (!_userManager.Exists(user.Email))
            {
                var result = await _userManager.CreateUserAsync(user, dto.Password);

                if (result)
                {
                    var token = await _tokenGeneratorService.GenerateTokenAsync(user);
                    var response = new { Email = dto.Email, PasswordLength = dto.Password.Length, Token = token };
                    var json = JsonConvert.SerializeObject(response, Formatting.Indented);

                    return Ok(json);
                }
            }
            else ModelState.AddModelError("Exists", "User with this email already exists!");

            return BadRequest(ModelState);
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost("[Action]")]
    public async Task<IActionResult> UserExists([FromBody] string email)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _userManager.ExistsAsync(email);

            return Ok(result);
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
