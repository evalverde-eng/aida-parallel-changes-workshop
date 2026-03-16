using Aida.ParallelChange.Api.Domain;
using Aida.ParallelChange.Api.Infrastructure.Http.Errors;
using Aida.ParallelChange.Api.Infrastructure.Http.Mappers.V2;

namespace Aida.ParallelChange.Api.Tests.Unit.Infrastructure.Http.Mappers.V2;

[TestFixture]
public sealed class CustomerContactV2RequestMapperTests
{
    [Test]
    public void ToDomain_returns_customer_contact_when_payload_is_valid()
    {
        var contact = CustomerContactV2RequestMapper.ToDomain(
            7,
            "Grace",
            "Hopper",
            "+1",
            "5550100",
            "grace.hopper@example.com");

        contact.CustomerId.ShouldBe(new CustomerId(7));
        contact.ContactName.ShouldBe(new ContactName("Grace Hopper"));
        contact.Phone.ShouldBe(new PhoneNumber("+1 5550100"));
    }

    [Test]
    public void ToDomain_throws_validation_error_when_payload_is_invalid()
    {
        var exception = Should.Throw<ApiRequestValidationException>(() =>
            CustomerContactV2RequestMapper.ToDomain(7, string.Empty, string.Empty, "+1", "5550100", "grace.hopper@example.com"));

        exception.Title.ShouldBe("Invalid request");
        exception.Message.ShouldStartWith("Contact name is required.");
    }
}
