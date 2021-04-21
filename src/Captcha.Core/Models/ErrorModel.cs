namespace Captcha.Core.Models;

public class ErrorModel(Exception ex)
{
    public string Type { get; } = ex.GetType().Name;
    public string Message { get; } = ex.Message;
    public string StackTrace { get; } = ex.ToString();
}
