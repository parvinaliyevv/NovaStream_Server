namespace NovaStream.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
    private readonly AppDbContext _dbContext;


    public HomeController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    [HttpGet("[Action]")]
    public async Task<IActionResult> Index()
    {
        await Task.CompletedTask;

        try
        {
            var videos = new List<BaseVideoDto>();

            videos.AddRange(_dbContext.Movies.ProjectToType<MovieDto>());
            videos.AddRange(_dbContext.Serials.ProjectToType<SerialDto>());

            // sort for popular hit videos

            var json = JsonConvert.SerializeObject(videos, Formatting.Indented);

            return Ok(json);
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("[Action]")]
    public async Task<IActionResult> Recommended()
    {
        await Task.CompletedTask;

        try
        {
            var videos = new List<BaseVideoDto>();

            videos.AddRange(_dbContext.Movies.Take(3).ProjectToType<MovieSearchDto>());
            videos.AddRange(_dbContext.Serials.Take(3).ProjectToType<SerialSearchDto>());

            var json = JsonConvert.SerializeObject(videos, Formatting.Indented);

            return Ok(json);
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
