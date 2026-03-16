using Aida.ParallelChange.Api.Infrastructure.Persistence.SqlServer.Transition;

namespace Aida.ParallelChange.Api.Tests.Unit.Infrastructure.Persistence.SqlServer;

[TestFixture]
public sealed class CustomerContactTransitionPolicyTests
{
    [TestCase("Maria Garcia", "Maria", "Garcia")]
    [TestCase("Ana Lopez Torres", "Ana", "Lopez Torres")]
    [TestCase("Single", "Single", "")]
    [TestCase("  Maria Garcia  ", "Maria", "Garcia")]
    public void SplitName_splits_by_first_space(string source, string expectedFirstName, string expectedLastName)
    {
        var parts = CustomerContactTransitionPolicy.SplitName(source);

        parts.FirstName.ShouldBe(expectedFirstName);
        parts.LastName.ShouldBe(expectedLastName);
    }

    [TestCase("+34 600123123", "+34", "600123123")]
    [TestCase("+44 123 456", "+44", "123 456")]
    [TestCase("600123123", "600123123", "")]
    [TestCase("  +34 600123123  ", "+34", "600123123")]
    public void SplitPhone_splits_by_first_space(string source, string expectedCountryCode, string expectedLocalNumber)
    {
        var parts = CustomerContactTransitionPolicy.SplitPhone(source);

        parts.CountryCode.ShouldBe(expectedCountryCode);
        parts.LocalNumber.ShouldBe(expectedLocalNumber);
    }

    [TestCase("Maria", "Garcia", "Maria Garcia")]
    [TestCase("Maria", "", "Maria")]
    [TestCase("Maria", "  Garcia  ", "Maria Garcia")]
    public void ComposeName_builds_legacy_contact_name(string firstName, string lastName, string expected)
    {
        var composedName = CustomerContactTransitionPolicy.ComposeName(firstName, lastName);

        composedName.ShouldBe(expected);
    }

    [TestCase("+34", "600123123", "+34 600123123")]
    [TestCase("+34", "", "+34")]
    [TestCase("", "600123123", "600123123")]
    [TestCase("  +34  ", "  600123123  ", "+34 600123123")]
    public void ComposePhone_builds_legacy_phone(string countryCode, string localNumber, string expected)
    {
        var composedPhone = CustomerContactTransitionPolicy.ComposePhone(countryCode, localNumber);

        composedPhone.ShouldBe(expected);
    }
}
