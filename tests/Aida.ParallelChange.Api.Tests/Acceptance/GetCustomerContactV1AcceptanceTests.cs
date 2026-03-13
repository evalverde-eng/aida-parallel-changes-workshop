using System.Net;
using System.Text.Json;
using Shouldly;

namespace Aida.ParallelChange.Api.Tests.Acceptance;

[TestFixture]
public sealed class GetCustomerContactV1AcceptanceTests
{
    [Test]
    public async Task Get_returns_legacy_json_api_document()
    {
        await using var factory = new TestApiFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/v1/customer-contacts/42");
        var body = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(body);
        var attributes = document.RootElement.GetProperty("data").GetProperty("attributes");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.ShouldBe("application/vnd.api+json");
        attributes.GetProperty("contactName").GetString().ShouldBe("María García");
        attributes.GetProperty("phone").GetString().ShouldBe("+34 600123123");
        attributes.GetProperty("email").GetString().ShouldBe("maria.garcia@example.com");
    }

    [Test]
    public async Task Get_returns_not_found_when_customer_does_not_exist()
    {
        await using var factory = new TestApiFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/v1/customer-contacts/9999");

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
