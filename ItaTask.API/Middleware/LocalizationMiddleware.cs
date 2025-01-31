using System.Globalization;

namespace ItaTask.API.Middleware;

public class LocalizationMiddleware(RequestDelegate next)
{
    private static readonly string[] SupportedLanguages = ["ka", "en"];
    private const string DefaultLanguage = "ka";

    public async Task InvokeAsync(HttpContext context)
    {
        var acceptLanguage = context.Request.Headers["Accept-Language"].FirstOrDefault();
        string selectedLanguage = DefaultLanguage;

        if (!string.IsNullOrEmpty(acceptLanguage))
        {
            var preferredLanguage = acceptLanguage
                .Split(',')[0]
                .Split('-')[0]
                .ToLowerInvariant();

            if (SupportedLanguages.Contains(preferredLanguage))
            {
                selectedLanguage = preferredLanguage;
            }
        }

        var culture = new CultureInfo(selectedLanguage);
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

        context.Items["SelectedLanguage"] = selectedLanguage;

        await next(context);
    }
}