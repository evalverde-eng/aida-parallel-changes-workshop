using System.Net;
using System.Text.Json;

namespace Aida.ParallelChange.Api.Tests.Acceptance;

[TestFixture]
public sealed class UpdateCustomerContactV2AcceptanceTests : SeededCustomerContactAcceptanceFixture
{
    [Test]
    public async Task Put_updates_customer_contact_through_structured_contract()
    {
        const string body = """
        {
          "firstName": "Ada",
          "lastName": "Lovelace",
          "phoneCountryCode": "+44",
          "phoneLocalNumber": "123456789",
          "email": "ada.lovelace@example.com"
        }
        """;

        using var request = CreateJsonApiRequest(HttpMethod.Put, "/api/v2/customer-contacts/42", body);

        var putResponse = await Client.SendAsync(request);
        var getResponse = await Client.GetAsync("/api/v2/customer-contacts/42");
        var getBody = await getResponse.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(getBody);
        var attributes = document.RootElement.GetProperty("data").GetProperty("attributes");

        putResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        getResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        attributes.GetProperty("firstName").GetString().ShouldBe("Ada");
        attributes.GetProperty("lastName").GetString().ShouldBe("Lovelace");
        attributes.GetProperty("phoneCountryCode").GetString().ShouldBe("+44");
        attributes.GetProperty("phoneLocalNumber").GetString().ShouldBe("123456789");
        attributes.GetProperty("email").GetString().ShouldBe("ada.lovelace@example.com");
    }

    [Test]
    public async Task Put_returns_not_found_when_customer_does_not_exist()
    {
        const string body = """
        {
          "firstName": "Ada",
          "lastName": "Lovelace",
          "phoneCountryCode": "+44",
          "phoneLocalNumber": "123456789",
          "email": "ada.lovelace@example.com"
        }
        """;

        using var request = CreateJsonApiRequest(HttpMethod.Put, "/api/v2/customer-contacts/9999", body);

        var response = await Client.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(responseBody);
        var error = document.RootElement.GetProperty("errors")[0];

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        error.GetProperty("status").GetString().ShouldBe("404");
        error.GetProperty("title").GetString().ShouldBe("Customer contact not found");
        error.GetProperty("detail").GetString().ShouldBe("Customer contact '9999' was not found.");
    }

    [Test]
    public async Task Put_returns_bad_request_when_customer_id_is_invalid()
    {
        const string body = """
        {
          "firstName": "Ada",
          "lastName": "Lovelace",
          "phoneCountryCode": "+44",
          "phoneLocalNumber": "123456789",
          "email": "ada.lovelace@example.com"
        }
        """;

        using var request = CreateJsonApiRequest(HttpMethod.Put, "/api/v2/customer-contacts/invalid", body);

        var response = await Client.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(responseBody);
        var error = document.RootElement.GetProperty("errors")[0];

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        error.GetProperty("status").GetString().ShouldBe("400");
        error.GetProperty("title").GetString().ShouldBe("Invalid customer id");
        error.GetProperty("detail").GetString().ShouldBe("Customer id must be a number greater than zero.");
    }
}
