namespace Captcha.FunctionalTests.Support;
using RestSharp;

public class TestBase : IDisposable
{
    protected RestClient Client { get; set; }

    protected TestBase(ScenarioContext scenarioContext) => Client = new RestClient(new RestClientOptions(TestConstants.ServiceUrl));

    public void Dispose()
    {
        Client.Dispose();
        GC.SuppressFinalize(this);
    }
}
