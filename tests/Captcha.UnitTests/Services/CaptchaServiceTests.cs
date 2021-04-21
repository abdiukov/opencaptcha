namespace Captcha.Core.Tests.Services;

using System.Threading.Tasks;
using Captcha.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Models;
using NUnit.Framework;

[TestFixture]
public class CaptchaServiceTests
{
    [Test]
    public async Task CreateCaptchaImageAsyncReturnsFileContentResultWithCorrectContentType()
    {
        // Arrange
        var config = new CaptchaConfigurationData
        {
            Text = "my text",
            Width = 100,
            Height = 100,
            Difficulty = CaptchaDifficulty.Medium
        };
        var captchaService = new CaptchaService();

        // Act
        var result = await captchaService.CreateCaptchaImageAsync(config);

        // Assert
        Assert.That(result, Is.InstanceOf<FileContentResult>(), "Result should be a FileContentResult.");
        Assert.Multiple(() =>
        {
            Assert.That(result.ContentType, Is.EqualTo("image/jpeg"), "ContentType should be image/jpeg.");
            Assert.That(result.FileContents, Is.Not.Null, "FileContents should not be null.");
        });
        Assert.That(result.FileContents, Is.Not.Empty, "FileContents should have a size greater than 0.");
    }
}
