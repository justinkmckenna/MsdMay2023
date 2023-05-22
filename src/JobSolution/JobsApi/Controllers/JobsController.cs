using Microsoft.AspNetCore.Mvc;

namespace JobsApi.Controllers;

[Route("jobs")]
[ApiController]
public class JobsController : ControllerBase
{
    private readonly JobManager _jobManager;

    public JobsController(JobManager jobManager)
    {
        _jobManager = jobManager;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllJobs()
    {
        CollectionResponse<JobItemModel> response = await _jobManager.GetAllCurrentJobsAsync();
        return Ok(response);
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult> GetAllJobs(string slug)
    {
        var response = await _jobManager.GetJobBySlugAsync(slug);
        return response is null ? NotFound() : Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult> CreateJob([FromBody] JobCreateItem request)
    {
        JobItemModel response = await _jobManager.CreateJobAsync(request);
        return StatusCode(201, response);
    }
}



