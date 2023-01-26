namespace NovaStream.API.Controllers;

[ApiController, Authorize]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IUserManager _userManager;


    public MovieController(AppDbContext dbContext, IUserManager userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }


    [HttpGet("[Action]"), AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        try
        {
            var movies = await _dbContext.Movies.ProjectToType<MovieShortDetailsDto>().ToListAsync();

            var builder = new StringBuilder();

            movies.ForEach(dto =>
            {
                var categories = _dbContext.MovieCategories.Include(mc => mc.Category).Where(mc => mc.MovieName == dto.Name).Select(mc => mc.Category.Name).ToList();

                try
                {
                    builder.Append($"{categories[0]} •");

                    for (int i = 1; i < categories.Count - 1; i++) builder.Append($" {categories[i]} •");

                    builder.Append($" {categories[categories.Count - 1]}");
                }
                catch
                {
                    continue;
                }

                dto.IsSerial = null;
                dto.Categories = builder.ToString();

                builder.Clear();
            });

            var jsonSerializerOptions = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };

            var json = JsonConvert.SerializeObject(movies, Formatting.Indented, jsonSerializerOptions);

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
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var movie = _dbContext.Movies.FirstOrDefault(m => m.Name == name)?.Adapt<MovieDetailsDto>();

            if (movie is null) return NotFound();

            var user = await _userManager.ReturnUserFromContextAsync(HttpContext);

            movie.IsMarked = _dbContext.MovieMarks.Any(mm => mm.MovieName == name && mm.UserId == user.Id);

            var jsonSerializerOptions = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };

            var json = JsonConvert.SerializeObject(movie, Formatting.Indented, jsonSerializerOptions);

            return Ok(json);
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("[Action]")]
    public async Task<IActionResult> ViewDetails([FromQuery] string name)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var movie = _dbContext.Movies.FirstOrDefault(m => m.Name == name)?.Adapt<MovieViewDetailsDto>();

            if (movie is null) return NotFound();

            var user = await _userManager.ReturnUserFromContextAsync(HttpContext);

            movie.IsMarked = _dbContext.MovieMarks.Any(mm => mm.MovieName == name && mm.UserId == user.Id);
            movie.TrailerUrl = null;

            var jsonSerializerOptions = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };

            var json = JsonConvert.SerializeObject(movie, Formatting.Indented, jsonSerializerOptions);

            return Ok(json);
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
