namespace Captcha.Core.Models;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public class CaptchaImage(CaptchaConfigurationData config)
{
    public Random RandomGenerator { get; set; } = new Random();
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

    public Bitmap Create()
    {
        // Setup all the drawing stuff needed
        var bitmap = new Bitmap(Width, Height, PixelFormat.Format16bppRgb555);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        var rectangle = new Rectangle(0, 0, Width, Height);

        // Do the drawing
        using var font = GetFontThatFitsRectangle(rectangle, graphics);
        FillInTheBackground(rectangle, graphics);
        DrawWarpedText(font, rectangle, graphics);
        AddRandomNoise(rectangle, graphics);

        return bitmap;
    }

    private void AddRandomNoise(Rectangle rectangle, Graphics graphics)
    {
        using var hatchBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.LightGray, Color.DarkGray);
        var max = Math.Max(rectangle.Width, rectangle.Height);
        for (var i = 0; i < (int)(rectangle.Width * rectangle.Height / Frequency); i++)
        {
            var x = RandomGenerator.Next(rectangle.Width);
            var y = RandomGenerator.Next(rectangle.Height);
            var width = RandomGenerator.Next(max / 50);
            var height = RandomGenerator.Next(max / 50);
            graphics.FillEllipse(hatchBrush, x, y, width, height);
        }
    }

    private void DrawWarpedText(Font font, Rectangle rectangle, Graphics graphics)
    {
        // Set up the text to be in the middle
        var format = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        // Create a path using the text and warp it randomly.
        using var path = new GraphicsPath();
        path.AddString(Text, font.FontFamily, (int)font.Style, font.Size, rectangle, format);
        var divisor = 4F;   // TODO: We could use this one day as a parameter = how much to warp the text by
        PointF[] points =
        [
            new(RandomGenerator.Next(rectangle.Width) / divisor, RandomGenerator.Next(rectangle.Height) / divisor),
            new(rectangle.Width - (RandomGenerator.Next(rectangle.Width) / divisor), RandomGenerator.Next(rectangle.Height) / divisor),
            new(RandomGenerator.Next(rectangle.Width) / divisor, rectangle.Height - (RandomGenerator.Next(rectangle.Height) / divisor)),
            new(rectangle.Width - (RandomGenerator.Next(rectangle.Width) / divisor), rectangle.Height - (RandomGenerator.Next(rectangle.Height) / divisor))
        ];
        using var matrix = new Matrix();
        matrix.Translate(0F, 0F);
        path.Warp(points, rectangle, matrix, WarpMode.Perspective, 0F);

        // Draw the text.
        using var hatchBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.LightGray, Color.DarkGray);
        graphics.FillPath(hatchBrush, path);
    }

    private Font GetFontThatFitsRectangle(Rectangle rectangle, Graphics graphics)
    {
        SizeF size;
        float fontSize = rectangle.Height;

        // Adjust the font size until the text fits within the image.
        do
        {
            fontSize--;
            var font = new Font(FamilyName, fontSize, FontStyle.Bold);
            size = graphics.MeasureString(Text, font);
        } while (size.Width > rectangle.Width);

        return new Font(FamilyName, fontSize, FontStyle.Bold);
    }

    private static void FillInTheBackground(Rectangle rectangle, Graphics graphics)
    {
        using var hatchBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White);
        graphics.FillRectangle(hatchBrush, rectangle);
    }
}
