namespace pact_test.xunit.tests;

using System.Net;
using PactNet;

public class UnitTest1
{
    private readonly IPactBuilderV4 _pactBuilder;

    public UnitTest1()
    {
        var pact = Pact.V4("API-Consumer", "API-Provider");
        _pactBuilder = pact.WithHttpInteractions();
    }

    [Fact]
    public async Task Test()
    {
        // Arrange
        _pactBuilder
            .UponReceiving("a request")
                .WithRequest(HttpMethod.Get, "/")
            .WillRespond()
                .WithStatus(HttpStatusCode.OK);

        await _pactBuilder.VerifyAsync(async ctx =>
        {
            var client = new HttpClient { BaseAddress = ctx.MockServerUri };

            var response = await client.GetAsync("/");

            response.EnsureSuccessStatusCode();
        });
    }
}