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
        try
        {
            var videos = new List<VideoShortDetaislDto>();
            var builder = new StringBuilder();

            List<string> categories;

            videos.AddRange(_dbContext.Movies.ProjectToType<MovieShortDetailsDto>());
            videos.AddRange(_dbContext.Serials.ProjectToType<SerialShortDetailsDto>());

            videos.Sort((a, b) => string.Compare(a.Name, b.Name));

            foreach (var dto in videos)
            {
                if (Convert.ToBoolean(dto.IsSerial))
                {
                    dto.SeasonCount = _dbContext.Seasons.Count(s => s.SerialName == dto.Name);

                    categories = await _dbContext.SerialCategories.Include(mc => mc.Category).Where(sc => sc.SerialName == dto.Name).Select(mc => mc.Category.Name).ToListAsync();
                }
                else categories = await _dbContext.MovieCategories.Include(mc => mc.Category).Where(mc => mc.MovieName == dto.Name).Select(mc => mc.Category.Name).ToListAsync();

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

                dto.Categories = builder.ToString();

                builder.Clear();
            }

            var jsonSerializerOptions = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };

            var json = JsonConvert.SerializeObject(videos, Formatting.Indented, jsonSerializerOptions);

            return Ok(json);
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
