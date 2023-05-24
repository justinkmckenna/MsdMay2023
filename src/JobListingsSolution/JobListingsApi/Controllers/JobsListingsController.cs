using DotNetCore.CAP;
using JobListingsApi.Models;
using Marten;
using Microsoft.AspNetCore.Mvc;
using DomainEvents = MessageContracts;

namespace JobListingsApi.Controllers
{
    public class JobsListingsController : ControllerBase
    {
        private readonly IDocumentSession _documentSession;
        private readonly ICapPublisher _publisher;

        public JobsListingsController(IDocumentSession documentSession, ICapPublisher publisher)
        {
            _documentSession = documentSession;
            _publisher = publisher;
        }

        [HttpPost("/job-listings/{slug}/openings")]
        public async Task<ActionResult> AddJobListing([FromRoute] string slug, [FromBody] JobListingCreateModel request)
        {
            var savedJob = await _documentSession.Query<JobModel>().Where(job => job.Id == slug).FirstOrDefaultAsync();
            if (savedJob != null)
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
                var domainEvent = new DomainEvents.JobListingCreated
                {
                    Id = jobToAdd.Id.ToString(),
                    JobId = jobToAdd.JobId,
                    JobName = jobToAdd.JobName,
                    OpeningStartDate = jobToAdd.OpeningStartDate,
                    SalaryRange = new DomainEvents.Salaryrange
                    {
                        Min = jobToAdd.SalaryRange.Min ?? 0,
                        Max = jobToAdd.SalaryRange.Max ?? 0
                    }
                };
                await _publisher.PublishAsync(DomainEvents.JobListingCreated.MessageId, domainEvent);
                return StatusCode(201, jobToAdd);
            } else
            {
                return NotFound();
            }
        }
    }
}
