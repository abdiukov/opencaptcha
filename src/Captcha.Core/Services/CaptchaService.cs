namespace Captcha.Core.Services;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Mvc;
using Models;
using SkiaSharp;

public class CaptchaService : ICaptchaService
{
    public async Task<FileContentResult> CreateCaptchaImageAsync(CaptchaConfigurationData config)
    {
        var image = new CaptchaImage(config);

        // Save the image to a memory stream so we can return it as a file
        await using var ms = new MemoryStream();
        using var data = image.Value.Encode(SKEncodedImageFormat.Jpeg, 100); // quality is set to 100, vary as needed
        data.SaveTo(ms);

        var imageBytes = ms.ToArray();

        // Return the image as a jpeg file
        return new FileContentResult(imageBytes, "image/jpeg");
    }
}
