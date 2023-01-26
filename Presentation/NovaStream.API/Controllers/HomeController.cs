namespace NovaStream.API.Controllers;

[ApiController, Authorize]
[Route("api/[controller]")]
public class HomeController: ControllerBase
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

            // videos.Sort();

            var json = $"\"videos\": {JsonConvert.SerializeObject(videos, Formatting.Indented)}";

            return Ok(json);
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
