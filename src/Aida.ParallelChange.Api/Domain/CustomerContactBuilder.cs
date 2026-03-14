namespace Aida.ParallelChange.Api.Domain;

public sealed class CustomerContactBuilder
{
    private const string MissingCustomerIdMessage = "Customer id must be configured before building customer contact.";
    private const string MissingContactNameMessage = "Contact name must be configured before building customer contact.";
    private const string MissingPhoneNumberMessage = "Phone number must be configured before building customer contact.";
    private const string MissingEmailAddressMessage = "Email address must be configured before building customer contact.";

    private CustomerId? _customerId;
    private ContactName? _contactName;
    private PhoneNumber? _phoneNumber;
    private EmailAddress? _emailAddress;

    private CustomerContactBuilder()
    {
    }

    public static CustomerContactBuilder Create()
    {
        return new CustomerContactBuilder();
    }

    public CustomerContactBuilder WithCustomerId(int customerId)
    {
        _customerId = new CustomerId(customerId);
        return this;
    }

    public CustomerContactBuilder WithContactName(string contactName)
    {
        _contactName = new ContactName(contactName);
        return this;
    }

    public CustomerContactBuilder WithPhoneNumber(string phoneNumber)
    {
        _phoneNumber = new PhoneNumber(phoneNumber);
        return this;
    }

    public CustomerContactBuilder WithEmailAddress(string emailAddress)
    {
        _emailAddress = new EmailAddress(emailAddress);
        return this;
    }

    public CustomerContact Build()
    {
        return new CustomerContact(
            _customerId ?? throw new InvalidOperationException(MissingCustomerIdMessage),
            _contactName ?? throw new InvalidOperationException(MissingContactNameMessage),
            _phoneNumber ?? throw new InvalidOperationException(MissingPhoneNumberMessage),
            _emailAddress ?? throw new InvalidOperationException(MissingEmailAddressMessage));
    }
}
