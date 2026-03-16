using Aida.ParallelChange.Api.Domain;
using Aida.ParallelChange.Api.Infrastructure.Http.Mappers.V2;

namespace Aida.ParallelChange.Api.Tests.Unit.Infrastructure.Http.Mappers.V2;

[TestFixture]
public sealed class CustomerContactV2ResponseMapperTests
{
    [Test]
    public void ToDocument_maps_customer_contact_into_structured_json_api_document()
    {
        var contact = CustomerContactBuilder.Create()
            .WithCustomerId(9)
            .WithContactName("Grace Hopper")
            .WithPhoneNumber("+1 5550100")
            .WithEmailAddress("grace.hopper@example.com")
            .Build();

        var document = CustomerContactV2ResponseMapper.ToDocument(contact);

        document.Data.Type.ShouldBe("customer-contacts");
        document.Data.Id.ShouldBe("9");
        document.Data.Attributes.FirstName.ShouldBe("Grace");
        document.Data.Attributes.LastName.ShouldBe("Hopper");
        document.Data.Attributes.PhoneCountryCode.ShouldBe("+1");
        document.Data.Attributes.PhoneLocalNumber.ShouldBe("5550100");
        document.Data.Attributes.Email.ShouldBe("grace.hopper@example.com");
    }
}
