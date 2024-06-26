namespace Captcha.FunctionalTests.Support;
using RestSharp;

public class TestBase
{
    protected RestClient Client { get; set; }
    protected ScenarioContext Context { get; set; }

    protected TestBase(ScenarioContext context)
    {
        Client = new RestClient(new RestClientOptions(TestConstants.ServiceUrl));
        Context = context;
    }
}
