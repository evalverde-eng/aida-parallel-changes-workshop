using Aida.ParallelChange.Api.Domain;
using Aida.ParallelChange.Api.Infrastructure.Http.Mappers.V1;

namespace Aida.ParallelChange.Api.Tests.Unit.Infrastructure.Http.Mappers.V1;

[TestFixture]
public sealed class CustomerContactV1ResponseMapperTests
{
    [Test]
    public void ToDocument_maps_customer_contact_into_json_api_document()
    {
        var contact = CustomerContactBuilder.Create()
            .WithCustomerId(9)
            .WithContactName("Grace Hopper")
            .WithPhoneNumber("+1 5550100")
            .WithEmailAddress("grace.hopper@example.com")
            .Build();

        var document = CustomerContactV1ResponseMapper.ToDocument(contact);

        document.Data.Type.ShouldBe("customer-contacts");
        document.Data.Id.ShouldBe("9");
        document.Data.Attributes.ContactName.ShouldBe("Grace Hopper");
        document.Data.Attributes.Phone.ShouldBe("+1 5550100");
        document.Data.Attributes.Email.ShouldBe("grace.hopper@example.com");
    }
}
