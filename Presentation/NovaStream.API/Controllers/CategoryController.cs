namespace NovaStream.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase 
{
    private readonly AppDbContext _dbContext;


    public CategoryController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    [HttpGet("[Action]")]
    public async Task<IActionResult> VideosByCategory([FromQuery] string name)
    {
        await Task.CompletedTask;

        try
        {
            var videos = new List<VideoSearchDto>();

            videos.AddRange(_dbContext.MovieCategories.Include(mc => mc.Movie).Include(mc => mc.Category).Where(mc => mc.Category.Name == name).Select(mc => mc.Movie).ProjectToType<MovieSearchDto>());
            videos.AddRange(_dbContext.SerialCategories.Include(sc => sc.Serial).Include(sc => sc.Category).Where(sc => sc.Category.Name == name).Select(sc => sc.Serial).ProjectToType<SerialSearchDto>());

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
