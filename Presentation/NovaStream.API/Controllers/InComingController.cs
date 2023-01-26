namespace NovaStream.API.Controllers;

[ApiController, Route("api/[controller]")]
public class InComingController : ControllerBase
{
    private readonly AppDbContext _dbContext;


    public InComingController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    [HttpGet("[Action]")]
    public async Task<IActionResult> Index()
    {
        await Task.CompletedTask;

        try
        {
            var inComings = new List<InComingDto>();

            inComings.AddRange(_dbContext.InComings.ProjectToType<InComingDto>());

            inComings.ForEach(dto =>
            {
                var builder = new StringBuilder();

                var categories = _dbContext.InComingCategories.Where(icc => icc.InComingVideoName == dto.Name).Select(icc => icc.Category.Name).ToList();

                for (int i = 0; i < categories.Count; i++)
                {
                    if (i == 0) builder.Append($"{categories[i]} •");
                    else if (i == categories.Count - 1) builder.Append($" {categories[i]}");
                    else builder.Append($" {categories[i]} •");
                }

                dto.Categories = builder.ToString();
            });

            var json = JsonConvert.SerializeObject(inComings, Formatting.Indented);

            return Ok(json);
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
