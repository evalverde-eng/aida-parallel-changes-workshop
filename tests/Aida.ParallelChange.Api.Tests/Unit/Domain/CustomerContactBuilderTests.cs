using Aida.ParallelChange.Api.Domain;

namespace Aida.ParallelChange.Api.Tests.Unit.Domain;

[TestFixture]
public sealed class CustomerContactBuilderTests
{
    [Test]
    public void Build_creates_contact_with_value_objects()
    {
        var contact = CustomerContactBuilder.Create()
            .WithCustomerId(7)
            .WithContactName("Grace Hopper")
            .WithPhoneNumber("+1 5550100")
            .WithEmailAddress("grace.hopper@example.com")
            .Build();

        contact.CustomerId.ShouldBe(new CustomerId(7));
        contact.ContactName.ShouldBe(new ContactName("Grace Hopper"));
        contact.Phone.ShouldBe(new PhoneNumber("+1 5550100"));
        contact.Email.ShouldBe(new EmailAddress("grace.hopper@example.com"));
    }

    [Test]
    public void Build_throws_when_any_invariant_is_invalid()
    {
        var exception = Should.Throw<ArgumentException>(() =>
            CustomerContactBuilder.Create()
                .WithCustomerId(7)
                .WithContactName(string.Empty)
                .WithPhoneNumber("+1 5550100")
                .WithEmailAddress("grace.hopper@example.com")
                .Build());

        exception.Message.ShouldStartWith("Contact name is required.");
    }
}
