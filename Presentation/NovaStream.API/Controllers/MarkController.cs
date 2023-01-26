namespace NovaStream.API.Controllers;

[ApiController, Authorize]
[Route("api/[controller]")]
public class MarkController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IUserManager _userManager;


    public MarkController(AppDbContext dbContext, IUserManager userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }


    [HttpPost("[Action]")]
    public async Task<IActionResult> MarkMovie([FromQuery] string name)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = _userManager.ReturnUserFromContext(HttpContext);

            if (user is null) return Unauthorized();

            var movie = _dbContext.Movies.FirstOrDefault(s => s.Name == name);

            if (movie is null) return NotFound();

            var IsMarked = _dbContext.MovieMarks.Any(ms => ms.MovieName == movie.Name && ms.UserEmail == user.Email);

            if (IsMarked)
            {
                ModelState.AddModelError("IsMarked", "This movie is alredy marked!");

                return BadRequest(ModelState);
            }

            var movieMark = new MovieMark(user.Email, movie.Name);

            await _dbContext.MovieMarks.AddAsync(movieMark);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost("[Action]")]
    public async Task<IActionResult> MarkSerial([FromQuery] string name)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = _userManager.ReturnUserFromContext(HttpContext);

            if (user is null) return Unauthorized();

            var serial = _dbContext.Serials.FirstOrDefault(s => s.Name == name);

            if (serial is null) return NotFound(ModelState);

            var IsMarked = _dbContext.SerialMarks.Any(ms => ms.SerialName == serial.Name && ms.UserEmail == user.Email);

            if (IsMarked)
            {
                ModelState.AddModelError("IsMarked", "This serial is alredy marked!");

                return BadRequest(ModelState);
            }

            var serialMark = new SerialMark(user.Email, serial.Name);

            await _dbContext.SerialMarks.AddAsync(serialMark);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("[Action]")]
    public async Task<IActionResult> GetMarkedVideos()
    {
        try
        {
            var user = await _userManager.ReturnUserFromContextAsync(HttpContext);

            if (user is null) return Unauthorized();

            var markedVideos = new List<MarkDto>();

            markedVideos.AddRange(_dbContext.MovieMarks.Include(mm => mm.Movie).Where(mm => mm.UserEmail == user.Email).Select(mm => new MarkDto(mm.Movie.ImageUrl) { Name = mm.MovieName }));
            markedVideos.AddRange(_dbContext.SerialMarks.Include(ms => ms.Serial).Where(ms => ms.UserEmail == user.Email).Select(ms => new MarkDto(ms.Serial.ImageUrl) { Name = ms.SerialName }));

            markedVideos.Sort((a, b) => string.Compare(a.Name, b.Name));

            var json = JsonConvert.SerializeObject(markedVideos, Formatting.Indented);

            return Ok(json);
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
