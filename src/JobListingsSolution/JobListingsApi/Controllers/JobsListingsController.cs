using JobListingsApi.Models;
using Marten;
using Microsoft.AspNetCore.Mvc;

namespace JobListingsApi.Controllers
{
    public class JobsListingsController : ControllerBase
    {
        private readonly IDocumentSession _documentSession;

        public JobsListingsController(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        [HttpPost("/job-listings/{slug}/openings")]
        public async Task<ActionResult> AddJobListing([FromRoute] string slug, [FromBody] JobListingCreateModel request)
        {
            var savedJob = await _documentSession.Query<JobModel>().Where(job => job.Id == slug).FirstOrDefaultAsync();
            if(savedJob != null)
            {
                var jobToAdd = new JobListingEntity
                {
                    JobId = slug,
                    JobName = savedJob.Title,
                    OpeningStartDate = request.OpeningStartDate,
                    SalaryRange = request.SalaryRange
                };
                _documentSession.Insert(jobToAdd);
                await _documentSession.SaveChangesAsync();
                return StatusCode(201, jobToAdd);
            } else
            {
                return NotFound();
            }
        }
    }
}
