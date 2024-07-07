namespace Captcha.Core.Models;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public class CaptchaImage
{
    public Random RandomGenerator { get; set; } = new Random();
    public string Text { get; set; }
    public int Width { get;set; }
    public int Height { get; set; }
    public string FamilyName { get; set; }
    public float Frequency { get; set; }

    public CaptchaImage(CaptchaConfigurationData config)
    {
        Text = config.Text;
        Width = config.Width;
        Height = config.Height;
        FamilyName = config.Font;
        Frequency = config.Difficulty switch
        {
            CaptchaDifficulty.Easy => 300F,
            CaptchaDifficulty.Medium => 100F,
            CaptchaDifficulty.Challenging => 30F,
            CaptchaDifficulty.Hard => 20F,
            _ => throw new ArgumentOutOfRangeException(nameof(config),
                $"Invalid value for Difficulty: {config.Difficulty}. The provided captcha difficulty is not supported.")
        };
    }

    public Bitmap Create()
    {
        // Create a new 16-bit bitmap image.
        var bitmap = new Bitmap(Width, Height, PixelFormat.Format16bppRgb555);

        // Create a graphics object for drawing.
        var graphics = Graphics.FromImage(bitmap);
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        var rect = new Rectangle(0, 0, Width, Height);

        // Fill in the background.
        var hatchBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White);
        graphics.FillRectangle(hatchBrush, rect);

        // Set up the text font.
        SizeF size;
        float fontSize = rect.Height + 1;
        Font font;

        // Adjust the font size until the text fits within the image.
        do
        {
            fontSize--;
            font = new Font(FamilyName, fontSize, FontStyle.Bold);
            size = graphics.MeasureString(Text, font);
        } while (size.Width > rect.Width);

        // Set up the text format.
        var format = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        // Create a path using the text and warp it randomly.
        var path = new GraphicsPath();
        path.AddString(Text, font.FontFamily, (int)font.Style, font.Size, rect, format);
        var v = 4F;
        PointF[] points =
        [
            new(RandomGenerator.Next(rect.Width) / v, RandomGenerator.Next(rect.Height) / v),
            new(rect.Width - (RandomGenerator.Next(rect.Width) / v), RandomGenerator.Next(rect.Height) / v),
            new(RandomGenerator.Next(rect.Width) / v, rect.Height - (RandomGenerator.Next(rect.Height) / v)),
            new(rect.Width - (RandomGenerator.Next(rect.Width) / v), rect.Height - (RandomGenerator.Next(rect.Height) / v))
        ];
        var matrix = new Matrix();
        matrix.Translate(0F, 0F);
        path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);

        // Draw the text.
        hatchBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.LightGray, Color.DarkGray);
        graphics.FillPath(hatchBrush, path);

        // Add some random noise.
        var m = Math.Max(rect.Width, rect.Height);
        for (var i = 0; i < (int)(rect.Width * rect.Height / Frequency); i++)
        {
            var x = RandomGenerator.Next(rect.Width);
            var y = RandomGenerator.Next(rect.Height);
            var w = RandomGenerator.Next(m / 50);
            var h = RandomGenerator.Next(m / 50);
            graphics.FillEllipse(hatchBrush, x, y, w, h);
        }

        // Clean up.
        font.Dispose();
        hatchBrush.Dispose();
        graphics.Dispose();

        return bitmap;
    }
}
