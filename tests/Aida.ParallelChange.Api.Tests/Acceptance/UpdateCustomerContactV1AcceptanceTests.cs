using System.Net;
using System.Text;
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

        putResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        getResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        getBody.ShouldContain(""contactName":"Ada Lovelace"");
        getBody.ShouldContain(""phone":"+44 123456789"");
        getBody.ShouldContain(""email":"ada.lovelace@example.com"");
    }
}
