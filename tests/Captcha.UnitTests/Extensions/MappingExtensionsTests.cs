namespace Captcha.UnitTests.Extensions;

using Captcha.Core.Extensions;
using Core.Models;
using NUnit.Framework;

[TestFixture]
public class MappingExtensionsTests
{
    [Test]
    public void ToDomainMapsTextCorrectly()
    {
        var request = new CaptchaRequest { Text = "test text" };
        var result = request.ToDomain();
        Assert.That(result.Text, Is.EqualTo("test text"));
    }

    [Test]
    public void ToDomainUsesDefaultWidthWhenNotProvided()
    {
        var request = new CaptchaRequest();
        var result = request.ToDomain();
        Assert.That(result.Width, Is.EqualTo(400));
    }

    [Test]
    public void ToDomainUsesDefaultHeightWhenNotProvided()
    {
        var request = new CaptchaRequest();
        var result = request.ToDomain();
        Assert.That(result.Height, Is.EqualTo(100));
    }

    [Test]
    public void ToDomainUsesDefaultDifficultyWhenNotProvided()
    {
        var request = new CaptchaRequest();
        var result = request.ToDomain();
        Assert.That(result.Difficulty, Is.EqualTo(CaptchaDifficulty.Medium));
    }


    [Test]
    public void ToDomainUsesProvidedWidthWhenGiven()
    {
        var request = new CaptchaRequest { Width = 500 };
        var result = request.ToDomain();
        Assert.That(result.Width, Is.EqualTo(500));
    }

    [Test]
    public void ToDomainUsesProvidedHeightWhenGiven()
    {
        var request = new CaptchaRequest { Height = 300 };
        var result = request.ToDomain();
        Assert.That(result.Height, Is.EqualTo(300));
    }

    [Test]
    public void ToDomainUsesProvidedDifficultyWhenGiven()
    {
        var request = new CaptchaRequest { Difficulty = CaptchaDifficulty.Hard };
        var result = request.ToDomain();
        Assert.That(result.Difficulty, Is.EqualTo(CaptchaDifficulty.Hard));
    }

    [Test]
    public void ToDomainMapsMultipleRequestsCorrectly()
    {
        var requests = new List<CaptchaRequest>
        {
            new() {
                Text = "test1",
                Width = 500,
                Height = 200,
                Difficulty = CaptchaDifficulty.Easy
            },
            new() {
                Text = "test2",
                Width = 600,
                Height = 300,
                Difficulty = CaptchaDifficulty.Hard
            },
        };

        var results = requests.Select(r => r.ToDomain()).ToList();
        Assert.Multiple(() =>
        {
            Assert.That(results[0].Text, Is.EqualTo(requests[0].Text));
            Assert.That(results[0].Width, Is.EqualTo(requests[0].Width));
            Assert.That(results[0].Height, Is.EqualTo(requests[0].Height));
            Assert.That(results[0].Difficulty, Is.EqualTo(requests[0].Difficulty));

            Assert.That(results[1].Text, Is.EqualTo(requests[1].Text));
            Assert.That(results[1].Width, Is.EqualTo(requests[1].Width));
            Assert.That(results[1].Height, Is.EqualTo(requests[1].Height));
            Assert.That(results[1].Difficulty, Is.EqualTo(requests[1].Difficulty));
        });
    }

    // Null object scenario
    [Test]
    public void ToDomainThrowsWhenRequestIsNull()
    {
        CaptchaRequest request = null;

        Assert.Throws<NullReferenceException>(() => request.ToDomain());
    }
}
