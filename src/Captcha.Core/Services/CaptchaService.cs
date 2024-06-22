namespace Captcha.Core.Services;

using Microsoft.AspNetCore.Mvc;
using Models;
using SkiaSharp;

public class CaptchaService : ICaptchaService
{
    public async Task<FileContentResult> CreateCaptchaImageAsync(CaptchaConfigurationData config)
    {
        var image = new CaptchaDrawingService();

        // Save the image to a memory stream so we can return it as a file
        await using var ms = new MemoryStream();
        using var data = image.GenerateImage(config).Encode(SKEncodedImageFormat.Webp, 30); // quality is set to 30, vary as needed
        data.SaveTo(ms);

        var imageBytes = ms.ToArray();

        // Return the image as a jpeg file
        return new FileContentResult(imageBytes, "image/jpeg");
    }


}
