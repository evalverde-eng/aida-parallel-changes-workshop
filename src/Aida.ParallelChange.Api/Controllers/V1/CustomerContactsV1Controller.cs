using Aida.ParallelChange.Api.Contracts.JsonApi;
using Aida.ParallelChange.Api.Contracts.V1;
using Aida.ParallelChange.Api.Infrastructure.InMemory;
using Aida.ParallelChange.Api.JsonApi;
using Microsoft.AspNetCore.Mvc;

namespace Aida.ParallelChange.Api.Controllers.V1;

[ApiController]
[Route("api/v1/customer-contacts")]
public sealed class CustomerContactsV1Controller : ControllerBase
{
    private readonly LegacyCustomerContactStore _store;

    public CustomerContactsV1Controller(LegacyCustomerContactStore store)
    {
        _store = store;
    }

    [HttpGet("{customerId:int}")]
    [Produces(JsonApiMediaTypes.JsonApi)]
    public ActionResult<CustomerContactV1Document> Get(int customerId)
    {
        var record = _store.FindById(customerId);

        if (record is null)
        {
            return NotFound();
        }

        var document = new CustomerContactV1Document
        {
            Data = new JsonApiResource<CustomerContactV1Attributes>
            {
                Type = "customer-contacts",
                Id = record.CustomerId.ToString(),
                Attributes = new CustomerContactV1Attributes
                {
                    ContactName = record.ContactName,
                    Phone = record.Phone,
                    Email = record.Email
                }
            }
        };

        Response.ContentType = JsonApiMediaTypes.JsonApi;

        return Ok(document);
    }
}
