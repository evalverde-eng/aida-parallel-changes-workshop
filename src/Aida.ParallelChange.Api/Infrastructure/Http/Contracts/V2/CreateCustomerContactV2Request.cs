namespace Aida.ParallelChange.Api.Infrastructure.Http.Contracts.V2;

public sealed class CreateCustomerContactV2Request
{
    public int CustomerId { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string PhoneCountryCode { get; init; } = string.Empty;
    public string PhoneLocalNumber { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
