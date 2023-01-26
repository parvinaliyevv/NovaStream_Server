namespace NovaStream.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly AppDbContext _dbContext;


    public SearchController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    [HttpGet("[Action]")]
    public async Task<IActionResult> Index()
    {
        await Task.CompletedTask;

        try
        {
            var videos = new List<VideoSearchDto>();

            videos.AddRange(_dbContext.Movies.ProjectToType<MovieSearchDto>());
            videos.AddRange(_dbContext.Serials.ProjectToType<SerialSearchDto>());

            videos.Sort((a, b) => string.Compare(a.Name, b.Name));

            var json = JsonConvert.SerializeObject(videos, Formatting.Indented);

            return Ok(json);
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
