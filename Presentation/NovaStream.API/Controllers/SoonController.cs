namespace NovaStream.API.Controllers;

[ApiController, Route("api/[controller]")]
public class SoonController : ControllerBase
{
    private readonly AppDbContext _dbContext;


    public SoonController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    [HttpGet("[Action]")]
    public async Task<IActionResult> Index()
    {
        try
        {
            var soons = await _dbContext.Soons.Include(s => s.Genres).ThenInclude(c => c.Genre).ToListAsync();

            soons.Sort((a, b) => DateTime.Compare(a.OutDate, b.OutDate));
            
            var dtos = soons.Adapt<List<SoonDto>>();

            var json = JsonConvert.SerializeObject(dtos, Formatting.Indented);

            return Ok(json);
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
