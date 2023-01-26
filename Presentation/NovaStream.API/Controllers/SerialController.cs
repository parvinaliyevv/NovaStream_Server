namespace NovaStream.API.Controllers;

[ApiController, Authorize]
[Route("api/[controller]")]
public class SerialController : ControllerBase
{
    private readonly AppDbContext _dbContext;


    public SerialController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    [HttpGet("[Action]")]
    public async Task<IActionResult> Index()
    {
        try
        {
            var serials = await _dbContext.Serials.ProjectToType<SerialDto>().ToListAsync();

            var json = $"\"serials\": {JsonConvert.SerializeObject(serials, Formatting.Indented)}";

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

            var serial = _dbContext.Serials.FirstOrDefault(s => s.Name == name)?.Adapt<SerialDetailsDto>();

            if (serial is not null)
            {
                var season = await _dbContext.Seasons.FirstOrDefaultAsync(s => s.Number == 1 && s.SerialName == name);

                serial.SeasonCount = _dbContext.Seasons.Count(s => s.SerialName == name);
                serial.Episodes = await _dbContext.Episodes.Where(e => e.SeasonId == season.Id).ProjectToType<EpisodeDto>().ToListAsync();
                serial.Categories = await _dbContext.SerialCategories.Include(sc => sc.Category).Where(sc => sc.SerialName == name).Select(sc => sc.Category).ProjectToType<CategoryDto>().ToListAsync();
                
                var json = JsonConvert.SerializeObject(serial, Formatting.Indented);

                return Ok(json);
            }
            
            return NotFound();
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("[Action]")]
    public async Task<IActionResult> Season([FromQuery] string name, [FromQuery] int seasonNumber)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var season = await _dbContext.Seasons.FirstOrDefaultAsync(s => s.Number == seasonNumber && s.SerialName == name);

            if (season is not null)
            {
                var episodes = await _dbContext.Episodes.Where(e => e.SeasonId == season.Id).ProjectToType<EpisodeDto>().ToListAsync();
                var json = JsonConvert.SerializeObject(episodes, Formatting.Indented);

                return Ok($"\"episodes\": {json}");
            }

            return NotFound();
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("[Action]")]
    public async Task<IActionResult> Episode([FromQuery] string name, [FromQuery] int seasonNumber, [FromQuery] int episodeNumber)
    {
        await Task.CompletedTask;

        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var season = await _dbContext.Seasons.FirstOrDefaultAsync(s => s.Number == seasonNumber && s.SerialName == name);
            
            if (season is not null)
            {
                var episode = _dbContext.Episodes.FirstOrDefault(e => e.Number == episodeNumber && e.SeasonId == season.Id);

                if (episode is not null) return Ok(episode.VideoPath);
            }

            return NotFound();
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
