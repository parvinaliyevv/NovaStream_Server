namespace NovaStream.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IUserManager _userManager;
    private readonly IMailManager _mailManager;
    private readonly IEncryptorService _passwordEncryptorService;
    private readonly ITokenGeneratorService _tokenGeneratorService;


    public AccountController(ITokenGeneratorService tokenGeneratorService, IUserManager userManager, IMailManager mailManager, IEncryptorService passwordEncryptorService)
    {
        _userManager = userManager;
        _tokenGeneratorService = tokenGeneratorService;
        _mailManager = mailManager;
        _passwordEncryptorService = passwordEncryptorService;
    }


    [HttpGet("[Action]")]
    public async Task<IActionResult> Exists([FromQuery] string email)
    {
        try
        {
            return Ok(await _userManager.ExistsAsync(email));
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
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
                var token = await _tokenGeneratorService.GenerateAuthorizeTokenAsync(user);
                var response = new { Nickname = user.Nickname, AvatarUrl = user.AvatarUrl, Email = dto.Email, PasswordLength = dto.Password.Length, Token = token };
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
                    var token = await _tokenGeneratorService.GenerateAuthorizeTokenAsync(user);
                    var response = new { Nickname = dto.Nickname, AvatarUrl = dto.AvatarUrl, Email = dto.Email, PasswordLength = dto.Password.Length, Token = token };
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

    [HttpPut("[Action]"), Authorize]
    public async Task<IActionResult> Edit([FromForm] EditUserDto dto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.ReturnUserFromContextAsync(HttpContext);

            if (user is null) return Unauthorized();

            dto.Adapt(user);

            var result = await _userManager.UpdateUserAsync(user);

            return Ok(result);
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPut("[Action]"), Authorize]
    public async Task<IActionResult> ChangePassword([FromForm] string oldPassword, [FromForm] string newPassword)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.ReturnUserFromContextAsync(HttpContext);

            if (user is null) return Unauthorized();

            if (!_userManager.CheckPassword(user, oldPassword))
                ModelState.AddModelError("Invalid password", "the entered password is not correct!");
            
            else if (_userManager.CheckOldPassword(user, newPassword)) 
                ModelState.AddModelError("Already used password", "you have already used this password");
            
            else
            {
                var result = await _userManager.ChangePasswordAsync(user, newPassword);

                return Ok(result);
            }

            return BadRequest(ModelState);
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpDelete("[Action]"), Authorize]
    public async Task<IActionResult> Delete([FromForm] string password)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.ReturnUserFromContextAsync(HttpContext);

            if (user is null) return Unauthorized();

            var result = await _userManager.DeleteUserAsync(user);

            return Ok(result);
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("[Action]")]
    public async Task<IActionResult> SendConfirmationPIN([FromQuery] string mail)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindUserByEmailAsync(mail);

            if (user is not null)
            {
                var confirmPIN = MailManager.CreateConfirmationPIN(4);

                await _mailManager.SendMailToAsync(confirmPIN, mail);

                user.ConfirmationPIN = confirmPIN;

                var result = await _userManager.UpdateUserAsync(user);

                return Ok(result);
            }

            return NotFound();
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("[Action]")]
    public async Task<IActionResult> CheckConfirmationPIN([FromQuery] string mail, [FromQuery] string pinCode)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindUserByEmailAsync(mail);

            if (user is not null)
            {
                bool result = user.ConfirmationPIN == pinCode;

                if (result) 
                {
                    user.ConfirmationPIN = "";
                    await _userManager.UpdateUserAsync(user);
                }

                return Ok(result);
            }

            return NotFound();
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("[Action]")]
    public async Task<IActionResult> DeleteConfirmationPIN([FromQuery] string mail)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindUserByEmailAsync(mail);

            if (user is not null)
            {
                user.ConfirmationPIN = "";

                var result = await _userManager.UpdateUserAsync(user);

                return Ok(result);
            }

            return NotFound();
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("[Action]")]
    public async Task<IActionResult> ResetPassword([FromQuery] string mail, [FromQuery] string password)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.ReturnUserFromContextAsync(HttpContext);

            if (user is null) return Unauthorized();

            else
            {
                user.PasswordHash = _passwordEncryptorService.EncryptPassword(password);
                user.OldPasswordHash = user.PasswordHash;

                var result = await _userManager.UpdateUserAsync(user);

                return Ok(result);
            }
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
