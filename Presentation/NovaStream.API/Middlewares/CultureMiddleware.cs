namespace NovaStream.API.Middlewares;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;


    public CultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }


    public async Task Invoke(HttpContext context)
    {
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("us");

        await _next.Invoke(context);
    }
}
