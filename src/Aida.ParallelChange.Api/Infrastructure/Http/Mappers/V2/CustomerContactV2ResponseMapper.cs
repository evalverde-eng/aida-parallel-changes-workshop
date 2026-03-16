using Aida.ParallelChange.Api.Domain;
using Aida.ParallelChange.Api.Infrastructure.Http.Contracts.JsonApi;
using Aida.ParallelChange.Api.Infrastructure.Http.Contracts.V2;

namespace Aida.ParallelChange.Api.Infrastructure.Http.Mappers.V2;

public static class CustomerContactV2ResponseMapper
{
    private const string ResourceType = "customer-contacts";
    private const char Separator = ' ';

    public static CustomerContactV2Document ToDocument(CustomerContact customerContact)
    {
        var nameParts = SplitValue(customerContact.ContactName.Value);
        var phoneParts = SplitValue(customerContact.Phone.Value);

        return new CustomerContactV2Document
        {
            Data = new JsonApiResource<CustomerContactV2Attributes>
            {
                Type = ResourceType,
                Id = customerContact.CustomerId.Value.ToString(),
                Attributes = new CustomerContactV2Attributes
                {
                    FirstName = nameParts.FirstPart,
                    LastName = nameParts.SecondPart,
                    PhoneCountryCode = phoneParts.FirstPart,
                    PhoneLocalNumber = phoneParts.SecondPart,
                    Email = customerContact.Email.Value
                }
            }
        };
    }

    private static SplitParts SplitValue(string input)
    {
        var normalizedInput = input.Trim();
        var separatorIndex = normalizedInput.IndexOf(Separator);

        if (separatorIndex < 0)
        {
            return new SplitParts(normalizedInput, string.Empty);
        }

        var firstPart = normalizedInput[..separatorIndex].Trim();
        var secondPart = normalizedInput[(separatorIndex + 1)..].Trim();

        return new SplitParts(firstPart, secondPart);
    }
}

public sealed record SplitParts(string FirstPart, string SecondPart);
