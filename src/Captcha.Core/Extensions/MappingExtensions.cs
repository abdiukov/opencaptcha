namespace Captcha.Core.Extensions;

using Models;

public static class MappingExtensions
{
    public static CaptchaConfigurationData ToDomain(this CaptchaRequest request) => new()
    {
        Text = request.Text,
        Width = request.Width ?? 400,
        Height = request.Height ?? 100,
        Font = "Arial Unicode MS",
        Difficulty = request.Difficulty ?? CaptchaDifficulty.Medium
    };
}
