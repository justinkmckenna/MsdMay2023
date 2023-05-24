using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using AclEvents = MessageContracts;
namespace HiringApi.Controllers;



public class HiringRequestsController : ControllerBase
{
    private readonly ICapPublisher _publisher;

    public HiringRequestsController(ICapPublisher publisher)
    {
        _publisher = publisher;
    }

    [HttpPost("/hiringrequests")]
    [CapSubscribe("HrAcl.HiringRequestCreated")]
    public async Task<ActionResult> ProcessHiringRequestAsync([FromBody] AclEvents.HrAclMessages.HiringRequest request)
    {
        if (request.EmailAddress.ToLower().Contains("geico.com"))
        {
            // Publish the sad path (HiringRequestDenied)
            return BadRequest("Not allowed to hire from there, sorry bud. ");
        }
        else
        {
            // Publish the happy path (EmployeeCreated?)
            var eventToPublish = new AclEvents.HrAclMessages.EmployeeHired
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailAddress = request.EmailAddress,
                JobId = request.JobId,

            };
            var headers = new Dictionary<string, string?>()
            {
                {"offering-id", request.OfferingId.ToString() }
            };
            await _publisher.PublishAsync("Hr.EmployeeHired", eventToPublish, headers);
            return Ok(request);



        }



    }
}