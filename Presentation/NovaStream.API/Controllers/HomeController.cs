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
            var videos = new List<VideoDto>();

            videos.AddRange(_dbContext.Movies.ProjectToType<MovieDto>());
            videos.AddRange(_dbContext.Serials.ProjectToType<SerialDto>());

            videos.Sort((a, b) => string.Compare(a.Name, b.Name));

            videos.ForEach(dto =>
            {
                var builder = new StringBuilder();

                List<string> categories;

                if (Convert.ToBoolean(dto.IsSerial))
                {
                    dto.SeasonCount = _dbContext.Seasons.Count(s => s.SerialName == dto.Name);

                    categories = _dbContext.SerialCategories.Include(mc => mc.Category).Where(sc => sc.SerialName == dto.Name).Select(mc => mc.Category.Name).ToList();
                }
                else
                    categories = _dbContext.MovieCategories.Include(mc => mc.Category).Where(mc => mc.MovieName == dto.Name).Select(mc => mc.Category.Name).ToList();

                for (int i = 0; i < categories.Count; i++)
                {
                    if (i == 0) builder.Append($"{categories[i]} •");
                    else if (i == categories.Count - 1) builder.Append($" {categories[i]}");
                    else builder.Append($" {categories[i]} •");
                }

                dto.Categories = builder.ToString();
            });

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
