namespace Captcha.Core.Extensions;
using System.Drawing;
using Models;

public static class MappingExtensions
{
    public static CaptchaConfigurationData ToDomain(this CaptchaRequest request) => new()
    {
        Text = request.Text,
        Width = request.Width ?? 400,
        Height = request.Height ?? 100,
        Font = "Brush Script",
        Difficulty = request.Difficulty ?? CaptchaDifficulty.Medium
    };
}
