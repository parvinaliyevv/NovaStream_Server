using NovaStream.Domain.Entities.Concrete;

namespace NovaStream.API.Controllers;

[ApiController, Authorize]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private readonly AppDbContext _context;


    public MovieController(AppDbContext context)
    {
        _context = context;
    }


    [HttpGet("[Action]")]
    public IActionResult Read()
    {
        var videos = _context.Videos.ProjectToType<VideoDto>();
        
        var json = JsonConvert.SerializeObject(videos);

        return Ok($"\"videos\": {json}");
    }

    [HttpGet("[Action]")]
    public IActionResult Details([FromQuery] string name)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var video = _context.Videos?.FirstOrDefault(v => v.Name == name)?.Adapt<DetailsVideoDto>();

        if (video != null)
        {
            var json = JsonConvert.SerializeObject(video, Formatting.Indented);

            return Ok(json);
        }
        else ModelState.AddModelError("Exists", "Video with this name doesn't exist");

        return BadRequest(ModelState);
    }
}