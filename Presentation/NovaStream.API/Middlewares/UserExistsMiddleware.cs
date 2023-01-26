namespace NovaStream.API.Middlewares;

public class UserExistsMiddleware
{
    private readonly RequestDelegate _next;


    public UserExistsMiddleware(RequestDelegate next)
    {
        _next = next;
    }


    public async Task Invoke(HttpContext context)
    {
        var userManager = context.RequestServices.GetService<IUserManager>();

        var user = userManager.ReturnUserFromContext(context);
        
        if (user is null) context.User = null;

        await _next(context);
    }
}
