namespace Captcha.Core.Models;

public class CaptchaConfigurationData
{
    public string Text { get; init; }
    public int Width { get; init; }
    public int Height { get; init; }
    public string Font { get; init; }
    public CaptchaDifficulty Difficulty { get; init; }
}
