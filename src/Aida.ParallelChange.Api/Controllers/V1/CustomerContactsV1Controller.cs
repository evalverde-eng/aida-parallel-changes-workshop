using Aida.ParallelChange.Api.Application.GetCustomerContact;
using Aida.ParallelChange.Api.Contracts.JsonApi;
using Aida.ParallelChange.Api.Contracts.V1;
using Aida.ParallelChange.Api.Domain;
using Aida.ParallelChange.Api.Infrastructure.InMemory;
using Aida.ParallelChange.Api.JsonApi;
using Microsoft.AspNetCore.Mvc;

namespace Aida.ParallelChange.Api.Controllers.V1;

[ApiController]
[Route("api/v1/customer-contacts")]
public sealed class CustomerContactsV1Controller : ControllerBase
{
    private readonly GetCustomerContactHandler _handler;
    private readonly InMemoryCustomerContactRepository _repository;

    public CustomerContactsV1Controller(GetCustomerContactHandler handler, InMemoryCustomerContactRepository repository)
    {
        _handler = handler;
        _repository = repository;
    }

    [HttpGet("{customerId:int}")]
    [Produces(JsonApiMediaTypes.JsonApi)]
    public async Task<ActionResult<CustomerContactV1Document>> Get(int customerId, CancellationToken cancellationToken)
    {
        var contact = await _handler.HandleAsync(new GetCustomerContactQuery(new CustomerId(customerId)), cancellationToken);

        if (contact is null)
        {
            return NotFound();
        }

        var document = new CustomerContactV1Document
        {
            Data = new JsonApiResource<CustomerContactV1Attributes>
            {
                Type = "customer-contacts",
                Id = contact.CustomerId.Value.ToString(),
                Attributes = new CustomerContactV1Attributes
                {
                    ContactName = contact.ContactName,
                    Phone = contact.Phone,
                    Email = contact.Email.Value
                }
            }
        };

        Response.ContentType = JsonApiMediaTypes.JsonApi;

        return Ok(document);
    }

    [HttpPut("{customerId:int}")]
    [Consumes(JsonApiMediaTypes.JsonApi)]
    public async Task<IActionResult> Put(int customerId, [FromBody] UpdateCustomerContactV1Request request, CancellationToken cancellationToken)
    {
        var customerContact = new CustomerContact(
            new CustomerId(customerId),
            request.ContactName,
            request.Phone,
            new EmailAddress(request.Email));

        await _repository.SaveAsync(customerContact, cancellationToken);

        return NoContent();
    }
}
