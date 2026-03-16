using System.Net;
using System.Text.Json;

namespace Aida.ParallelChange.Api.Tests.Acceptance;

[TestFixture]
public sealed class CreateCustomerContactV2AcceptanceTests : SeededCustomerContactAcceptanceFixture
{
    [Test]
    public async Task Post_creates_customer_contact_through_structured_contract()
    {
        const string body = """
        {
          "customerId": 88,
          "firstName": "Grace",
          "lastName": "Hopper",
          "phoneCountryCode": "+1",
          "phoneLocalNumber": "5550100",
          "email": "grace.hopper@example.com"
        }
        """;

        using var request = CreateJsonApiRequest(HttpMethod.Post, "/api/v2/customer-contacts", body);

        var postResponse = await Client.SendAsync(request);
        var getResponse = await Client.GetAsync("/api/v2/customer-contacts/88");
        var getBody = await getResponse.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(getBody);
        var attributes = document.RootElement.GetProperty("data").GetProperty("attributes");

        postResponse.StatusCode.ShouldBe(HttpStatusCode.Created);
        postResponse.Headers.Location.ShouldNotBeNull();
        postResponse.Headers.Location!.OriginalString.ShouldBe("/api/v2/customer-contacts/88");
        getResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        attributes.GetProperty("firstName").GetString().ShouldBe("Grace");
        attributes.GetProperty("lastName").GetString().ShouldBe("Hopper");
        attributes.GetProperty("phoneCountryCode").GetString().ShouldBe("+1");
        attributes.GetProperty("phoneLocalNumber").GetString().ShouldBe("5550100");
        attributes.GetProperty("email").GetString().ShouldBe("grace.hopper@example.com");
    }

    [Test]
    public async Task Post_returns_bad_request_when_payload_is_invalid()
    {
        const string body = """
        {
          "customerId": 89,
          "firstName": "",
          "lastName": "",
          "phoneCountryCode": "+1",
          "phoneLocalNumber": "5550101",
          "email": "invalid"
        }
        """;

        using var request = CreateJsonApiRequest(HttpMethod.Post, "/api/v2/customer-contacts", body);

        var response = await Client.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(responseBody);
        var error = document.RootElement.GetProperty("errors")[0];

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        error.GetProperty("status").GetString().ShouldBe("400");
        error.GetProperty("title").GetString().ShouldBe("Invalid request");
        error.GetProperty("detail").GetString().ShouldBe("Contact name is required. (Parameter 'contactName')");
    }

    [Test]
    public async Task Post_returns_conflict_when_customer_already_exists()
    {
        const string body = """
        {
          "customerId": 42,
          "firstName": "Existing",
          "lastName": "Contact",
          "phoneCountryCode": "+1",
          "phoneLocalNumber": "5550102",
          "email": "existing@example.com"
        }
        """;

        using var request = CreateJsonApiRequest(HttpMethod.Post, "/api/v2/customer-contacts", body);

        var response = await Client.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(responseBody);
        var error = document.RootElement.GetProperty("errors")[0];

        response.StatusCode.ShouldBe(HttpStatusCode.Conflict);
        error.GetProperty("status").GetString().ShouldBe("409");
        error.GetProperty("title").GetString().ShouldBe("Customer contact already exists");
        error.GetProperty("detail").GetString().ShouldBe("Customer contact '42' already exists.");
    }
}
