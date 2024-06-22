namespace Captcha.UnitTests.Models;

using System;
using Captcha.Core.Models;
using NUnit.Framework;

[TestFixture]
public class CaptchaImageTests
{
    [Test]
    public void CaptchaImageConstructedWithConfigGeneratesImageOfCorrectDimensions()
    {
        // Arrange
        var config = new CaptchaConfigurationData
        {
            Text = "text",
            Width = 200,
            Height = 100,
            Font = "Arial",
            Difficulty = CaptchaDifficulty.Easy
        };

        // Act
        var captchaImage = new CaptchaImage(config);
        Assert.Multiple(() =>
        {

            // Assert
            Assert.That(captchaImage.Value.Width, Is.EqualTo(config.Width));
            Assert.That(captchaImage.Value.Height, Is.EqualTo(config.Height));
        });
    }

    [Test]
    public void CaptchaImageConstructedWithDifferentConfigGeneratesImageOfCorrectDimensions()
    {
        // Arrange
        var config = new CaptchaConfigurationData
        {
            Text = "text",
            Width = 500,
            Height = 300,
            Font = "Arial",
            Difficulty = CaptchaDifficulty.Hard
        };

        // Act
        var captchaImage = new CaptchaImage(config);
        Assert.Multiple(() =>
        {

            // Assert
            Assert.That(captchaImage.Value.Width, Is.EqualTo(config.Width));
            Assert.That(captchaImage.Value.Height, Is.EqualTo(config.Height));
        });
    }

    [Test]
    public void CaptchaImageConstructedWithUnknownDifficultyThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var config = new CaptchaConfigurationData
        {
            Text = "Test",
            Width = 200,
            Height = 100,
            Font = "Arial",
            Difficulty = (CaptchaDifficulty)999  // Any non-defined value
        };

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new CaptchaImage(config));
    }

    [Test]
    public void CaptchaImageConstructedWithNegativeHeightThrowsArgumentException()
    {
        // Arrange
        var config = new CaptchaConfigurationData()
        {
            Text = "Test",
            Width = 500,
            Height = -100,
            Font = "Arial",
            Difficulty = CaptchaDifficulty.Easy
        };

        // Act & Assert
        Assert.Throws<Exception>(() => new CaptchaImage(config));
    }


    [Test]
    public void CaptchaImageConstructedWithValidConfigValueIsNotNull()
    {
        // Arrange
        var config = new CaptchaConfigurationData
        {
            Text = "Test",
            Width = 200,
            Height = 100,
            Font = "Arial",
            Difficulty = CaptchaDifficulty.Easy
        };

        // Act
        var captchaImage = new CaptchaImage(config);

        // Assert
        Assert.That(captchaImage.Value, Is.Not.Null);
    }

}
