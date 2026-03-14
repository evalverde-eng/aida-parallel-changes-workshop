namespace Aida.ParallelChange.Api.Application.CreateCustomerContact;

public sealed class CustomerContactAlreadyExistsException : Exception
{
    public int CustomerId { get; }

    public CustomerContactAlreadyExistsException(int customerId)
        : base($"Customer contact '{customerId}' already exists.")
    {
        CustomerId = customerId;
    }
}
