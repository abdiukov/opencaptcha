namespace Captcha.FunctionalTests.StepDefinitions;

using Core.Models;
using RestSharp;
using SkiaSharp;
using Support;
using TechTalk.SpecFlow;

[Binding]
public class CaptchaSteps(ScenarioContext scenarioContext) : TestBase(scenarioContext)
{
    private CaptchaRequest _request;
    private RestResponse _response;

    [Given(@"I have a captcha request with following parameters:")]
    public void GivenIHaveACaptchaRequestWithFollowingParameters(Table table)
    {
        var row = table.Rows[0];

        _request = new CaptchaRequest
        {
            Text = row["Text"],
            Width = string.IsNullOrEmpty(row["Width"]) ? null : int.Parse(row["Width"]),
            Height = string.IsNullOrEmpty(row["Height"]) ? null : int.Parse(row["Height"]),
            Difficulty = string.IsNullOrEmpty(row["Difficulty"]) ? null
                : Enum.Parse<CaptchaDifficulty>(row["Difficulty"], true)
        };
    }

    [When(@"I send the request to the Create endpoint of the CaptchaController")]
    public async Task WhenISendTheRequestToTheCreateEndpointOfTheCaptchaController()
    {
        var request = new RestRequest("captcha") // calls localhost/captcha
            { RequestFormat = DataFormat.Json, Method = Method.Post };
        request.AddJsonBody(_request);
        _response = await Client.ExecuteAsync(request);
    }


    [Then(@"I expect a captcha image to be returned with the following attributes:")]
    public void ThenIExpectACaptchaImageToBeReturnedWithTheFollowingAttributes(Table table)
    {
        var row = table.Rows[0];
        using var ms = new MemoryStream(_response.RawBytes);
        using var s = new SKManagedStream(ms);
        var img = SKBitmap.Decode(s);

        var expectedWidth = int.Parse(row["Width"]);
        var expectedHeight = int.Parse(row["Height"]);

        img.Width.Equals(expectedWidth);
        img.Height.Equals(expectedHeight);
    }
}
