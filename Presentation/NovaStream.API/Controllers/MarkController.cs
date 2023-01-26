using NovaStream.Domain.Entities.Concrete;

namespace NovaStream.API.Controllers;

[ApiController, Authorize]
[Route("api/[controller]")]
public class MarkController : ControllerBase
{
    private readonly AppDbContext _dbContext;


    public MarkController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    [HttpPost("[Action]")]
    public async Task<IActionResult> MarkMovie([FromQuery] string name)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = GetDataBAZA();

            if (user is not null)
            {
                var movie = _dbContext.Movies.FirstOrDefault(m => m.Name == name);

                if (movie is not null)
                {
                    if (_dbContext.MovieMarks.Any(mm => mm.MovieName == movie.Name && mm.UserEmail == user.Email))
                    {
                        ModelState.AddModelError("Marked", "This movie is alredy marked!");
                        return BadRequest(ModelState);
                    }
                    else
                    {
                        await _dbContext.MovieMarks.AddAsync(new () { UserEmail = user.Email, MovieName = movie.Name });                 
                        await _dbContext.SaveChangesAsync();

                        return Ok();
                    }
                    
                }

                ModelState.AddModelError("NotFound", "Movie with this name is not found!");
            }
            else ModelState.AddModelError("NotFound", "This user is not found!");

            return NotFound(ModelState);
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

            var user = GetDataBAZA();

            if (user is not null)
            {
                var serial = _dbContext.Serials.FirstOrDefault(s => s.Name == name);

                if (serial is not null)
                {
                    if (_dbContext.SerialMarks.Any(ms => ms.SerialName == serial.Name && ms.UserEmail == user.Email))
                    {
                        ModelState.AddModelError("Marked", "This movie is alredy marked!");
                        return BadRequest(ModelState);
                    }
                    else
                    {
                        await _dbContext.SerialMarks.AddAsync(new SerialMark() { UserEmail = user.Email, SerialName = serial.Name });
                        await _dbContext.SaveChangesAsync();

                        return Ok();
                    }                    
                }

                ModelState.AddModelError("NotFound", "Serial with this name is not found!");
            }
            else ModelState.AddModelError("NotFound", "This user is not found!");

            return NotFound(ModelState);
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
            var user = GetDataBAZA();

            if (user is not null)
            {
                var markedVideos = new List<string>();

                markedVideos.AddRange(await _dbContext.MovieMarks.Where(mm => mm.UserEmail == user.Email).Select(mm => mm.MovieName).ToListAsync());
                markedVideos.AddRange(await _dbContext.SerialMarks.Where(ms => ms.UserEmail == user.Email).Select(ms => ms.SerialName).ToListAsync());

                markedVideos.Sort();

                var json = $"\"markedVideos\": {JsonConvert.SerializeObject(markedVideos, Formatting.Indented)}";

                return Ok(json);
            }

            return NotFound();
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    private User? GetDataBAZA()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity != null)
        {
            var userClaims = identity.Claims;
            return new User
            {
                Email = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
            };
        }
        return null;
    }
}
