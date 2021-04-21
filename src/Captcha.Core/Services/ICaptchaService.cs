namespace Captcha.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Models;

public interface ICaptchaService
{
    Task<FileContentResult> CreateCaptchaImageAsync(CaptchaConfigurationData config);
}
