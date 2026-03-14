namespace Aida.ParallelChange.Api.Application.UpdateCustomerContact;

public sealed class CustomerContactNotFoundException : Exception
{
    public int CustomerId { get; }

    public CustomerContactNotFoundException(int customerId)
        : base($"Customer contact '{customerId}' was not found.")
    {
        CustomerId = customerId;
    }
}
