using Aida.ParallelChange.Api.Domain;
using Aida.ParallelChange.Api.Infrastructure.Http.Errors;

namespace Aida.ParallelChange.Api.Infrastructure.Http.Mappers.V2;

public static class CustomerContactV2RequestMapper
{
    private const string InvalidRequestTitle = "Invalid request";

    public static CustomerContact ToDomain(int customerId, string firstName, string lastName, string phoneCountryCode, string phoneLocalNumber, string email)
    {
        try
        {
            return CustomerContactBuilder.Create()
                .WithCustomerId(customerId)
                .WithContactName(ComposeName(firstName, lastName))
                .WithPhoneNumber(ComposePhone(phoneCountryCode, phoneLocalNumber))
                .WithEmailAddress(email)
                .Build();
        }
        catch (Exception exception) when (exception is ArgumentException or ArgumentOutOfRangeException)
        {
            throw new ApiRequestValidationException(InvalidRequestTitle, exception.Message);
        }
    }

    private static string ComposeName(string firstName, string lastName)
    {
        var normalizedFirstName = firstName.Trim();
        var normalizedLastName = lastName.Trim();

        if (string.IsNullOrWhiteSpace(normalizedLastName))
        {
            return normalizedFirstName;
        }

        return $"{normalizedFirstName} {normalizedLastName}";
    }

    private static string ComposePhone(string countryCode, string localNumber)
    {
        var normalizedCountryCode = countryCode.Trim();
        var normalizedLocalNumber = localNumber.Trim();

        if (string.IsNullOrWhiteSpace(normalizedLocalNumber))
        {
            return normalizedCountryCode;
        }

        if (string.IsNullOrWhiteSpace(normalizedCountryCode))
        {
            return normalizedLocalNumber;
        }

        return $"{normalizedCountryCode} {normalizedLocalNumber}";
    }
}
