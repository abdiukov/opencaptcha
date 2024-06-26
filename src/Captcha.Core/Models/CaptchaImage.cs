namespace Captcha.Core.Models;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public class CaptchaImage
{
    private readonly string _text;
    private readonly int _width;
    private readonly int _height;
    private readonly string _familyName;
    private readonly float _frequency;
    private readonly Random _random = new();

    public Bitmap Value { get; private set; }

    public CaptchaImage(CaptchaConfigurationData config)
    {
        _text = config.Text;
        _width = config.Width;
        _height = config.Height;
        _familyName = config.Font;

        _frequency = config.Difficulty switch
        {
            CaptchaDifficulty.Easy => 300F,
            CaptchaDifficulty.Medium => 100F,
            CaptchaDifficulty.Challenging => 30F,
            CaptchaDifficulty.Hard => 20F,
            _ => throw new ArgumentOutOfRangeException(nameof(config),
                $"Invalid value for Difficulty: {config.Difficulty}. The provided captcha difficulty is not supported.")
        };
        GenerateImage();
    }

    //Code I found that generates image
    private void GenerateImage()
    {
        // Create a new 32-bit bitmap image.
        var bitmap = new Bitmap(_width, _height, PixelFormat.Format16bppRgb555);

        // Create a graphics object for drawing.
        var g = Graphics.FromImage(bitmap);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var rect = new Rectangle(0, 0, _width, _height);

        // Fill in the background.
        var hatchBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White);
        g.FillRectangle(hatchBrush, rect);

        // Set up the text font.
        SizeF size;
        float fontSize = rect.Height + 1;
        Font font;

        // Adjust the font size until the text fits within the image.
        do
        {
            fontSize--;
            font = new Font(_familyName, fontSize, FontStyle.Bold);
            size = g.MeasureString(_text, font);
        } while (size.Width > rect.Width);

        // Set up the text format.
        var format = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        // Create a path using the text and warp it randomly.
        var path = new GraphicsPath();
        path.AddString(_text, font.FontFamily, (int)font.Style, font.Size, rect, format);
        var v = 4F;
        PointF[] points =
        [
            new(_random.Next(rect.Width) / v, _random.Next(rect.Height) / v),
            new(rect.Width - (_random.Next(rect.Width) / v), _random.Next(rect.Height) / v),
            new(_random.Next(rect.Width) / v, rect.Height - (_random.Next(rect.Height) / v)),
            new(rect.Width - (_random.Next(rect.Width) / v), rect.Height - (_random.Next(rect.Height) / v))
        ];
        var matrix = new Matrix();
        matrix.Translate(0F, 0F);
        path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);

        // Draw the text.
        hatchBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.LightGray, Color.DarkGray);
        g.FillPath(hatchBrush, path);

        // Add some random noise.
        var m = Math.Max(rect.Width, rect.Height);
        for (var i = 0; i < (int)(rect.Width * rect.Height / _frequency); i++)
        {
            var x = _random.Next(rect.Width);
            var y = _random.Next(rect.Height);
            var w = _random.Next(m / 50);
            var h = _random.Next(m / 50);
            g.FillEllipse(hatchBrush, x, y, w, h);
        }

        // Clean up.
        font.Dispose();
        hatchBrush.Dispose();
        g.Dispose();

        // Set the image.
        Value = bitmap;
    }
}
