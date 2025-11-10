namespace eShop.WebApp.Middleware;

public class ThemeMiddleware
{
    private readonly RequestDelegate _next;

    public ThemeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ThemeService themeService)
    {
        var theme = ThemeService.GetThemeFromCookie(context);
        themeService.SetTheme(theme);

        await _next(context);
    }
}

public static class ThemeMiddlewareExtensions
{
    public static IApplicationBuilder UseThemeMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ThemeMiddleware>();
    }
}
