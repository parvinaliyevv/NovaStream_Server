namespace NovaStream.API.Controllers;

[ApiController, Authorize]
[Route("api/[controller]")]
public class SerialController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IUserManager _userManager;


    public SerialController(AppDbContext dbContext, IUserManager userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }


    [HttpGet("[Action]"), AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        try
        {
            var serials = await _dbContext.Serials.ProjectToType<SerialShortDetailsDto>().ToListAsync();

            var builder = new StringBuilder();

            serials.ForEach(dto =>
            {
                var categories = _dbContext.SerialCategories.Include(mc => mc.Category).Where(mc => mc.SerialName == dto.Name).Select(mc => mc.Category.Name).ToList();

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
                dto.SeasonCount = _dbContext.Seasons.Count(s => s.SerialName == dto.Name);

                builder.Clear();
            });

            var jsonSerializerOptions = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };

            var json = JsonConvert.SerializeObject(serials, Formatting.Indented, jsonSerializerOptions);

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

            if (serial is null) return NotFound();

            var season = await _dbContext.Seasons.FirstOrDefaultAsync(s => s.Number == 1 && s.SerialName == name);

            var user = _userManager.ReturnUserFromContext(HttpContext);

            serial.IsMarked = _dbContext.SerialMarks.Any(mm => mm.SerialName == name && mm.UserId == user.Id);
            serial.SeasonCount = _dbContext.Seasons.Count(s => s.SerialName == name);
            serial.Episodes = await _dbContext.Episodes.Where(e => e.SeasonId == season.Id).ProjectToType<EpisodeDto>().ToListAsync();

            var json = JsonConvert.SerializeObject(serial, Formatting.Indented);

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

            var serial = _dbContext.Serials.FirstOrDefault(s => s.Name == name)?.Adapt<SerialViewDetailsDto>();

            if (serial is null) return NotFound();

            var season = await _dbContext.Seasons.FirstOrDefaultAsync(s => s.Number == 1 && s.SerialName == name);

            var user = _userManager.ReturnUserFromContext(HttpContext);

            serial.IsMarked = _dbContext.SerialMarks.Any(mm => mm.SerialName == name && mm.UserId == user.Id);
            serial.SeasonCount = _dbContext.Seasons.Count(s => s.SerialName == name);
            serial.TrailerUrl = null;

            var jsonSerializerOptions = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };

            var json = JsonConvert.SerializeObject(serial, Formatting.Indented, jsonSerializerOptions);

            return Ok(json);
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("[Action]")]
    public async Task<IActionResult> Season([FromQuery] string name, [FromQuery] int seasonNumber = 1)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var season = await _dbContext.Seasons.Include(s => s.Episodes).FirstOrDefaultAsync(s => s.Number == seasonNumber && s.SerialName == name);

            if (season is null) return NotFound();

            var episodes = season.Episodes.Adapt<ICollection<EpisodeDto>>();
            var json = JsonConvert.SerializeObject(episodes, Formatting.Indented);

            return Ok(json);
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("[Action]")]
    public async Task<IActionResult> Episode([FromQuery] string name, [FromQuery] int seasonNumber = 1, [FromQuery] int episodeNumber = 1)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var season = await _dbContext.Seasons.Include(s => s.Episodes).FirstOrDefaultAsync(s => s.Number == seasonNumber && s.SerialName == name);

            if (season is not null)
            {
                var episode = season.Episodes.FirstOrDefault(e => e.Number == episodeNumber);

                if (episode is not null) return Ok(episode.VideoUrl);
            }

            return NotFound();
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
