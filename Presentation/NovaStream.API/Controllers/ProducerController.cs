namespace NovaStream.API.Controllers;

[ApiController, Authorize]
[Route("api/[controller]")]
public class ProducerController : ControllerBase
{
    private readonly AppDbContext _dbContext;


    public ProducerController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    [HttpGet("[Action]")]
    public async Task<IActionResult> Videos([FromQuery] int id)
    {
        await Task.CompletedTask;

        try
        {
            var videos = new List<BaseVideoDto>();

            videos.AddRange(_dbContext.Serials.Include(s => s.Producer).Where(s => s.ProducerId == id).ProjectToType<SerialDto>());
            videos.AddRange(_dbContext.Movies.Include(s => s.Producer).Where(m => m.ProducerId == id).ProjectToType<MovieDto>());

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
