using System.Net;
using System.Text.Json;

namespace Aida.ParallelChange.Api.Tests.Acceptance;

[TestFixture]
public sealed class GetCustomerContactV2AcceptanceTests : SeededCustomerContactAcceptanceFixture
{
    [Test]
    public async Task Get_returns_structured_json_api_document()
    {
        var response = await Client.GetAsync("/api/v2/customer-contacts/42");
        var body = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(body);
        var attributes = document.RootElement.GetProperty("data").GetProperty("attributes");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.ShouldBe(JsonApiMediaType);
        attributes.GetProperty("firstName").GetString().ShouldBe("Maria");
        attributes.GetProperty("lastName").GetString().ShouldBe("Garcia");
        attributes.GetProperty("phoneCountryCode").GetString().ShouldBe("+34");
        attributes.GetProperty("phoneLocalNumber").GetString().ShouldBe("600123123");
        attributes.GetProperty("email").GetString().ShouldBe("maria.garcia@example.com");
    }

    [Test]
    public async Task Get_returns_not_found_when_customer_does_not_exist()
    {
        var response = await Client.GetAsync("/api/v2/customer-contacts/9999");
        var body = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(body);
        var error = document.RootElement.GetProperty("errors")[0];

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        response.Content.Headers.ContentType?.MediaType.ShouldBe(JsonApiMediaType);
        error.GetProperty("status").GetString().ShouldBe("404");
        error.GetProperty("title").GetString().ShouldBe("Customer contact not found");
        error.GetProperty("detail").GetString().ShouldBe("Customer contact '9999' was not found.");
    }

    [Test]
    public async Task Get_returns_bad_request_when_customer_id_is_invalid()
    {
        var response = await Client.GetAsync("/api/v2/customer-contacts/invalid-id");
        var body = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(body);
        var error = document.RootElement.GetProperty("errors")[0];

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType?.MediaType.ShouldBe(JsonApiMediaType);
        error.GetProperty("status").GetString().ShouldBe("400");
        error.GetProperty("title").GetString().ShouldBe("Invalid customer id");
        error.GetProperty("detail").GetString().ShouldBe("Customer id must be a number greater than zero.");
    }
}
