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
        var created = captchaImage.Create();
        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(created.Width, Is.EqualTo(config.Width));
            Assert.That(created.Height, Is.EqualTo(config.Height));
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
        var created = captchaImage.Create();
        Assert.Multiple(() =>
        {

            // Assert
            Assert.That(created.Width, Is.EqualTo(config.Width));
            Assert.That(created.Height, Is.EqualTo(config.Height));
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
        Assert.Throws<ArgumentOutOfRangeException>(() => new CaptchaImage(config).Create());
    }


    [Test]
    public void CaptchaImageConstructedWithZeroWidthThrowsArgumentException()
    {
        // Arrange
        var config = new CaptchaConfigurationData()
        {
            Text = "Test",
            Width = 0,
            Height = 100,
            Font = "Arial",
            Difficulty = CaptchaDifficulty.Easy
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new CaptchaImage(config).Create());
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
        Assert.Throws<ArgumentException>(() => new CaptchaImage(config).Create());
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
        var created = captchaImage.Create();
        // Assert
        Assert.That(created, Is.Not.Null);
    }

}
