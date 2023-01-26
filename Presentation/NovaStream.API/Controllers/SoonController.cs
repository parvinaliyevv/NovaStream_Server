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
            var soons = new List<SoonDto>();
            var builder = new StringBuilder();

            soons.AddRange(_dbContext.Soons.ProjectToType<SoonDto>());

            foreach (var dto in soons)
            {
                var categories = await _dbContext.SoonCategories.Include(sc => sc.Category).Where(icc => icc.SoonName == dto.Name).Select(icc => icc.Category.Name).ToListAsync();

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

            var json = JsonConvert.SerializeObject(soons, Formatting.Indented);

            return Ok(json);
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
