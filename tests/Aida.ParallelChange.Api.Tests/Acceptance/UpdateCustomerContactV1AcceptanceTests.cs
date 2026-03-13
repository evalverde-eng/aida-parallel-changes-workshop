using System.Net;
using System.Text;
using System.Text.Json;
using Shouldly;

namespace Aida.ParallelChange.Api.Tests.Acceptance;

[TestFixture]
public sealed class UpdateCustomerContactV1AcceptanceTests
{
    [Test]
    public async Task Put_updates_customer_contact_through_legacy_contract()
    {
        await using var factory = new TestApiFactory();
        using var client = factory.CreateClient();

        const string body = """
        {
          "contactName": "Ada Lovelace",
          "phone": "+44 123456789",
          "email": "ada.lovelace@example.com"
        }
        """;

        using var request = new HttpRequestMessage(HttpMethod.Put, "/api/v1/customer-contacts/42")
        {
            Content = new StringContent(body, Encoding.UTF8, "application/vnd.api+json")
        };

        var putResponse = await client.SendAsync(request);
        var getResponse = await client.GetAsync("/api/v1/customer-contacts/42");
        var getBody = await getResponse.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(getBody);
        var attributes = document.RootElement.GetProperty("data").GetProperty("attributes");

        putResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        getResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        attributes.GetProperty("contactName").GetString().ShouldBe("Ada Lovelace");
        attributes.GetProperty("phone").GetString().ShouldBe("+44 123456789");
        attributes.GetProperty("email").GetString().ShouldBe("ada.lovelace@example.com");
    }
}
