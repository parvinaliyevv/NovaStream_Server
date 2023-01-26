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
    public async Task<IActionResult> Index()
    {
        try
        {
            var movies = await _dbContext.Movies.ProjectToType<MovieDto>().ToListAsync();

            var json = $"\"movies\": {JsonConvert.SerializeObject(movies, Formatting.Indented)}";

            return Ok(json);
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("[Action]")]
    public async Task<IActionResult> Details([FromQuery] string name)
    {
        await Task.CompletedTask;

        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var movie = _dbContext.Movies.FirstOrDefault(m => m.Name == name)?.Adapt<MovieDetailsDto>();

            if (movie is not null)
            {
                var json = JsonConvert.SerializeObject(movie, Formatting.Indented);

                return Ok(json);
            }

            return NotFound();
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
