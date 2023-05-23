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
    public async Task<ActionResult> GetJobBySlug(string slug)
    {
        var response = await _jobManager.GetJobBySlugAsync(slug);
        return response is null ? NotFound() : Ok(response);
    }

    [HttpHead("{slug}")]
    public async Task<ActionResult> GetJobExistsBySlug(string slug)
    {
        var exists = await _jobManager.CheckForJobAsync(slug);
        return exists ? Ok() : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult> CreateJob([FromBody] JobCreateItem request)
    {
        JobItemModel response = await _jobManager.CreateJobAsync(request);
        return StatusCode(201, response);
    }
}



