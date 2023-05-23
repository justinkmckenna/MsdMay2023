using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using DomainEvents = MessageContracts.JobsApi;
namespace JobListingsApi.Controllers;

public class MessageSubscriberController : ControllerBase
{
    private readonly ILogger<MessageSubscriberController> _logger;


    public MessageSubscriberController(ILogger<MessageSubscriberController> logger)
    {
        _logger = logger;
    }

    [HttpPost("cap-stuff")]
    [CapSubscribe("JobsApi.JobCreated")]
    public async Task<ActionResult> GetNewJob([FromBody] DomainEvents.JobCreated request)
    {
        _logger.LogInformation($"Got a job created request {request.Id}, {request.Title}");
        return Ok();
    }
}