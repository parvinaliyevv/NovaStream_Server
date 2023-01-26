namespace NovaStream.API.Controllers;

[ApiController, Authorize]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private readonly AppDbContext _dbContext;


    public MovieController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    [HttpGet("[Action]")]
    public IActionResult Read()
    {
        try
        {
            var videos = _dbContext.Videos.ProjectToType<VideoDto>();
            var json = $"\"videos\": {JsonConvert.SerializeObject(videos, Formatting.Indented)}";

            return Ok(json);
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("[Action]")]
    public IActionResult Details([FromQuery] string name)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var video = _dbContext.Videos?.FirstOrDefault(v => v.Name == name)?.Adapt<DetailsVideoDto>();

            if (video != null)
            {
                var json = JsonConvert.SerializeObject(video, Formatting.Indented);

                return Ok(json);
            }
            else ModelState.AddModelError("Exists", "Video with this name doesn't exist");

            return BadRequest(ModelState);
        }
        catch
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
