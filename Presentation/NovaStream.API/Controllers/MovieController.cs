namespace NovaStream.API.Controllers;

[ApiController, Authorize]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private readonly AppDbContext _dbContext;


    public MovieController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    [HttpGet("[Action]")]
    public IActionResult Read()
    {
        try
        {
            var videos = _dbContext.Movies.ProjectToType<MovieDto>();
            var json = $"\"videos\":{JsonConvert.SerializeObject(videos)}";

            return Ok(json);
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("[Action]")]
    public IActionResult Details([FromQuery] string name)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var video = _dbContext.Movies?.FirstOrDefault(v => v.Name == name)?.Adapt<DetailsMovieDto>();

            if (video != null)
            {
                var json = JsonConvert.SerializeObject(video);

                return Ok(json);
            }
            else ModelState.AddModelError("Exists", "Movie with this name doesn't exist");

            return BadRequest(ModelState);
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
