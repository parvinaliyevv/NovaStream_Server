namespace NovaStream.API.Controllers;

[ApiController, Authorize]
[Route("api/[controller]")]
public class ActorController : ControllerBase
{
    private readonly AppDbContext _dbContext;


    public ActorController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("[Action]")]
    public async Task<IActionResult> Videos([FromQuery] int id)
    {
        try
        {
            var videos = new List<BaseVideoDto>();

            videos.AddRange(_dbContext.MovieActors.Include(ma => ma.Actor).Where(ma => ma.ActorId == id).Select(ma => ma.Movie).ProjectToType<MovieDto>());
            videos.AddRange(_dbContext.SerialActors.Include(sa => sa.Actor).Where(sa => sa.ActorId == id).Select(sa => sa.Serial).ProjectToType<SerialDto>());

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
