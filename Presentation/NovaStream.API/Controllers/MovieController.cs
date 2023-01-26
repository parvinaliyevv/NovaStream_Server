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
            var movies = await _dbContext.Movies.ProjectToType<MovieDto>().ToListAsync();

            movies.ForEach(dto =>
            {
                var builder = new StringBuilder();

                var categories = _dbContext.MovieCategories.Include(mc => mc.Category).Where(mc => mc.MovieName == dto.Name).Select(mc => mc.Category.Name).ToList();

                for (int i = 0; i < categories.Count; i++)
                {
                    if (i == 0) builder.Append($"{categories[i]} •");
                    else if (i == categories.Count - 1) builder.Append($" {categories[i]}");
                    else builder.Append($" {categories[i]} •");
                }

                dto.IsSerial = null;
                dto.Categories = builder.ToString();
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

            movie.IsMarked = _dbContext.MovieMarks.Any(mm => mm.MovieName == name && mm.UserEmail == user.Email);

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
