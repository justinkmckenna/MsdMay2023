using JobListingsApi.Adapters;
using JobListingsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobListingsApi.Controllers
{
    [ApiController]
    public class JobListingsRpcController : ControllerBase
    {
        private readonly JobsApiHttpAdapter _adapter;

        public JobListingsRpcController(JobsApiHttpAdapter adapter)
        {
            _adapter = adapter;
        }

        [HttpPost("job-listings-rpc/{job}/openings")]
        public async Task<ActionResult> AddJobListing([FromRoute] string job, [FromBody] JobListingCreateModel request)
        {
            var jobExists = await _adapter.JobExistsAsync(job);
            return jobExists ? Ok(job) : NotFound("Can't find job with that title. Sorry boss.");
        }
    }
}
