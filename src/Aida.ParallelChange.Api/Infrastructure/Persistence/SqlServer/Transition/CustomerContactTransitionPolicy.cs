namespace Aida.ParallelChange.Api.Infrastructure.Persistence.SqlServer.Transition;

public static class CustomerContactTransitionPolicy
{
    private const char Separator = ' ';

    public static NameParts SplitName(string contactName)
    {
        var normalizedName = contactName.Trim();
        var separatorIndex = normalizedName.IndexOf(Separator);

        if (separatorIndex < 0)
        {
            return new NameParts(normalizedName, string.Empty);
        }

        var firstName = normalizedName[..separatorIndex].Trim();
        var lastName = normalizedName[(separatorIndex + 1)..].Trim();

        return new NameParts(firstName, lastName);
    }

    public static PhoneParts SplitPhone(string phone)
    {
        var normalizedPhone = phone.Trim();
        var separatorIndex = normalizedPhone.IndexOf(Separator);

        if (separatorIndex < 0)
        {
            return new PhoneParts(normalizedPhone, string.Empty);
        }

        var countryCode = normalizedPhone[..separatorIndex].Trim();
        var localNumber = normalizedPhone[(separatorIndex + 1)..].Trim();

        return new PhoneParts(countryCode, localNumber);
    }

    public static string ComposeName(string firstName, string lastName)
    {
        var normalizedFirstName = firstName.Trim();
        var normalizedLastName = lastName.Trim();

        if (string.IsNullOrWhiteSpace(normalizedLastName))
        {
            return normalizedFirstName;
        }

        return $"{normalizedFirstName} {normalizedLastName}";
    }

    public static string ComposePhone(string countryCode, string localNumber)
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

public sealed record NameParts(string FirstName, string LastName);

public sealed record PhoneParts(string CountryCode, string LocalNumber);
