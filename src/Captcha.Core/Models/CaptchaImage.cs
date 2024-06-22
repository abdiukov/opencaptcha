namespace Captcha.Core.Models;
using SkiaSharp;

public class CaptchaImage(CaptchaConfigurationData config)
{
    public string Text { get; set; } = config.Text;
    public int Width { get; set; } = config.Width;
    public int Height { get; set; } = config.Height;
    public string FamilyName { get; set; } = config.Font;
    public float Frequency { get; set; } = config.Difficulty switch
    {
        CaptchaDifficulty.Easy => 300F,
        CaptchaDifficulty.Medium => 100F,
        CaptchaDifficulty.Challenging => 30F,
        CaptchaDifficulty.Hard => 20F,
        _ => throw new ArgumentOutOfRangeException(nameof(config),
            $"Invalid value for Difficulty: {config.Difficulty}. The provided captcha difficulty is not supported.")
    };

    public Random RandomGenerator { get; set; } = new Random();

    public SKBitmap GenerateImage()
    {
        var bitmap = new SKBitmap(Width, Height, SKColorType.Bgra8888, SKAlphaType.Premul);
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.Transparent);  // Clear the canvas with a transparent background

        var rect = new SKRect(0, 0, Width, Height);

        // Draw a unique hatch pattern
        DrawUniqueHatch(canvas, rect, SKColors.DarkGray, SKColors.WhiteSmoke, true);

        AdjustFontSizeToFit(canvas, Text, rect, FamilyName);
        DrawWarpText(canvas, Text, rect, FamilyName);
        AddRandomNoise(canvas, rect, Frequency);

        return bitmap;
    }


    private void DrawWarpText(SKCanvas canvas, string text, SKRect rect, string familyName)
    {
        // Calculate appropriate font size
        var fontSize = AdjustFontSizeToFit(canvas, text, rect, familyName);

        // Set up text painting
        using var textPaint = new SKPaint
        {
            Typeface = SKTypeface.FromFamilyName(familyName),
            TextSize = fontSize,
            Color = SKColors.DarkGray,
            IsAntialias = true,
        };

        // Create a text path
        var path = new SKPath();

        // Generate the text path using the text paint
        var textPath = textPaint.GetTextPath(text, 0, 0);

        // Find the text width and height to center it
        var textWidth = textPaint.MeasureText(text);
        var metrics = textPaint.FontMetrics;
        var textHeight = metrics.Descent - metrics.Ascent;

        // Set initial position to center the text
        var x = rect.MidX - (textWidth / 2);
        var y = rect.MidY - (textHeight / 2) - metrics.Ascent; // Adjust for baseline

        // Apply translation to the text path to move it to the desired position
        var translateMatrix = SKMatrix.CreateTranslation(x, y);
        textPath.Transform(translateMatrix);

        // Add the transformed text path to the main path
        path.AddPath(textPath);

        // Now, apply any additional transformations you want on 'path', e.g., warping
        // Random transformation example (similar to previous warp examples)
        var skewX = RandomGenerator.Next(-100, 100) / 1000.0f;
        var skewY = RandomGenerator.Next(-100, 100) / 1000.0f;
        float transX = RandomGenerator.Next(-20, 20);
        float transY = RandomGenerator.Next(-20, 20);

        var warpMatrix = SKMatrix.CreateIdentity();
        warpMatrix.SkewX = skewX;
        warpMatrix.SkewY = skewY;
        warpMatrix.TransX = transX;
        warpMatrix.TransY = transY;

        warpMatrix.Persp0 = RandomGenerator.Next(-100, 100) / 50000.0f; // Horizontal perspective
        warpMatrix.Persp1 = RandomGenerator.Next(-100, 100) / 50000.0f; // Vertical perspective
        warpMatrix.Persp2 = 1 + (RandomGenerator.Next(-100, 100) / 50000.0f); // Perspective division factor


        path.Transform(warpMatrix);

        // Draw the warped text on the canvas
        canvas.DrawPath(path, textPaint);
    }

    private static float AdjustFontSizeToFit(SKCanvas canvas, string text, SKRect rect, string familyName)
    {
        var fontSize = rect.Height;
        using var paint = new SKPaint
        {
            Typeface = SKTypeface.FromFamilyName(familyName),
            TextAlign = SKTextAlign.Center
        };

        // Decrease the font size until the text width is less than the rectangle width
        do
        {
            paint.TextSize = fontSize--;
            var textWidth = paint.MeasureText(text);
            if (textWidth <= rect.Width)
            {
                break;
            }
        } while (fontSize > 0);

        return fontSize;
    }


    private static void DrawUniqueHatch(SKCanvas canvas, SKRect rect, SKColor color1, SKColor color2, bool large = false)
    {
        var rnd = new Random();
        var dotCount = large ? 50 : 100;  // Fewer, larger dots for 'LargeConfetti'
        var minSize = large ? 3 : 1;
        var maxSize = large ? 10 : 5;

        // Draw the background
        canvas.DrawRect(rect, new SKPaint { Color = color2 });

        // Draw confetti
        for (var i = 0; i < dotCount; i++)
        {
            var paint = new SKPaint { Color = color1 };
            var x = rnd.Next((int)rect.Left, (int)rect.Right);
            var y = rnd.Next((int)rect.Top, (int)rect.Bottom);
            var size = rnd.Next(minSize, maxSize);
            canvas.DrawOval(new SKRect(x, y, x + size, y + size), paint);
        }
    }


    private void AddRandomNoise(SKCanvas canvas, SKRect rect, float frequency)
    {
        var m = Math.Max((int)rect.Width, (int)rect.Height);

        // Create a paint object for the noise ellipses
        using var paint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.DarkGray // You can customize the color or make it random
        };

        // Calculate the number of noise ellipses based on the specified frequency
        var noiseCount = (int)(rect.Width * rect.Height / frequency);

        for (var i = 0; i < noiseCount; i++)
        {
            var x = RandomGenerator.Next((int)rect.Left, (int)rect.Right);
            var y = RandomGenerator.Next((int)rect.Top, (int)rect.Bottom);
            var w = RandomGenerator.Next(m / 50);  // Width of the ellipse
            var h = RandomGenerator.Next(m / 50);  // Height of the ellipse

            // Draw an ellipse at the random position
            canvas.DrawOval(new SKRect(x, y, x + w, y + h), paint);
        }
    }

}
