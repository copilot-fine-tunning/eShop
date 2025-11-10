namespace eShop.WebApp.Services;

public class ThemeService
{
    private const string ThemeCookieName = "theme";
    private string _currentTheme = "light";

    public event Action? OnThemeChanged;

    public string CurrentTheme
    {
        get => _currentTheme;
        private set
        {
            if (_currentTheme != value)
            {
                _currentTheme = value;
                OnThemeChanged?.Invoke();
            }
        }
    }

    public void SetTheme(string theme)
    {
        CurrentTheme = theme;
    }

    public void ToggleTheme()
    {
        CurrentTheme = CurrentTheme == "light" ? "dark" : "light";
    }

    public static string GetThemeFromCookie(HttpContext httpContext)
    {
        return httpContext.Request.Cookies[ThemeCookieName] ?? "light";
    }

    public static void SetThemeCookie(HttpContext httpContext, string theme)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = false, // JavaScript needs to read it for instant updates
            SameSite = SameSiteMode.Lax,
            MaxAge = TimeSpan.FromDays(365),
            Secure = httpContext.Request.IsHttps,
            IsEssential = true
        };

        httpContext.Response.Cookies.Append(ThemeCookieName, theme, cookieOptions);
    }

    public static string CookieName => ThemeCookieName;
}
