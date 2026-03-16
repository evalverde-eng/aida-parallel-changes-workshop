using Aida.ParallelChange.Api.Application.CreateCustomerContact;
using Aida.ParallelChange.Api.Application.GetCustomerContact;
using Aida.ParallelChange.Api.Application.UpdateCustomerContact;
using Aida.ParallelChange.Api.Infrastructure.Http.Contracts.V2;
using Aida.ParallelChange.Api.Infrastructure.Http.JsonApi;
using Aida.ParallelChange.Api.Infrastructure.Http.Mappers.V1;
using Aida.ParallelChange.Api.Infrastructure.Http.Mappers.V2;
using Microsoft.AspNetCore.Mvc;

namespace Aida.ParallelChange.Api.Infrastructure.Http.Controllers.V2;

[ApiController]
[Route("api/v2/customer-contacts")]
public sealed class CustomerContactsV2Controller : ControllerBase
{
    private const string CustomerContactsLocationFormat = "/api/v2/customer-contacts/{0}";

    private readonly GetCustomerContactHandler _getHandler;
    private readonly CreateCustomerContactHandler _createHandler;
    private readonly UpdateCustomerContactHandler _updateHandler;

    public CustomerContactsV2Controller(
        GetCustomerContactHandler getHandler,
        CreateCustomerContactHandler createHandler,
        UpdateCustomerContactHandler updateHandler)
    {
        _getHandler = getHandler;
        _createHandler = createHandler;
        _updateHandler = updateHandler;
    }

    [HttpGet("{customerId}")]
    [Produces(JsonApiMediaTypes.JsonApi)]
    [ProducesResponseType(typeof(CustomerContactV2Document), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(string customerId, CancellationToken cancellationToken)
    {
        var parsedCustomerId = CustomerIdRouteParser.Parse(customerId);
        var result = await _getHandler.HandleAsync(new GetCustomerContactQuery(parsedCustomerId), cancellationToken);

        if (result is FindCustomerContactResult.NotFound notFound)
        {
            throw new CustomerContactNotFoundException(notFound.CustomerId.Value);
        }

        var found = (FindCustomerContactResult.Found)result;
        Response.ContentType = JsonApiMediaTypes.JsonApi;
        return Ok(CustomerContactV2ResponseMapper.ToDocument(found.CustomerContact));
    }

    [HttpPost]
    [Consumes(JsonApiMediaTypes.JsonApi)]
    [Produces(JsonApiMediaTypes.JsonApi)]
    [ProducesResponseType(typeof(CustomerContactV2Document), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Post([FromBody] CreateCustomerContactV2Request request, CancellationToken cancellationToken)
    {
        var customerContact = CustomerContactV2RequestMapper.ToDomain(
            request.CustomerId,
            request.FirstName,
            request.LastName,
            request.PhoneCountryCode,
            request.PhoneLocalNumber,
            request.Email);

        await _createHandler.HandleAsync(new CreateCustomerContactCommand(customerContact), cancellationToken);

        var location = string.Format(CustomerContactsLocationFormat, customerContact.CustomerId.Value);
        Response.ContentType = JsonApiMediaTypes.JsonApi;
        return Created(location, CustomerContactV2ResponseMapper.ToDocument(customerContact));
    }

    [HttpPut("{customerId}")]
    [Consumes(JsonApiMediaTypes.JsonApi)]
    [Produces(JsonApiMediaTypes.JsonApi)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(string customerId, [FromBody] UpdateCustomerContactV2Request request, CancellationToken cancellationToken)
    {
        var parsedCustomerId = CustomerIdRouteParser.Parse(customerId);
        var customerContact = CustomerContactV2RequestMapper.ToDomain(
            parsedCustomerId.Value,
            request.FirstName,
            request.LastName,
            request.PhoneCountryCode,
            request.PhoneLocalNumber,
            request.Email);

        await _updateHandler.HandleAsync(new UpdateCustomerContactCommand(customerContact), cancellationToken);

        return NoContent();
    }
}
