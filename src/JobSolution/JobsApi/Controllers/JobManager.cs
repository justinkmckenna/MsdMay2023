using DotNetCore.CAP;
using Marten;
using DomainEvents = MessageContracts.JobsApi;
using SlugGenerators;

namespace JobsApi.Controllers
{
    public class JobManager
    {
        private readonly SlugGenerator _slugGenerator;
        private readonly IDocumentStore _documentStore;
        private readonly ICapPublisher _publisher;

        public JobManager(SlugGenerator slugGenerator, IDocumentStore documentStore, ICapPublisher publisher)
        {
            _slugGenerator = slugGenerator;
            _documentStore = documentStore;
            _publisher = publisher;
        }

        public async Task<JobItemModel> CreateJobAsync(JobCreateItem request)
        {
            var jobToSave = new JobEntity
            {
                Title = request.Title,
                Slug = await _slugGenerator.GenerateSlugForAsync(request.Title),
                Description = request.Description,
            };
            using var session = _documentStore.LightweightSession();
            session.Insert(jobToSave);
            await session.SaveChangesAsync();
            var response = new JobItemModel
            {
                Id = jobToSave.Slug,
                Title = jobToSave.Title,
                Description = jobToSave.Description,
            };
            var domainEvent = new DomainEvents.JobCreated
            {
                Id = response.Id,
                Title = response.Title,
                Description = response.Description
            };
            await _publisher.PublishAsync(DomainEvents.JobCreated.MessageId, domainEvent);
            return response;
        }

        public async Task<CollectionResponse<JobItemModel>> GetAllCurrentJobsAsync()
        {
            using var session = _documentStore.LightweightSession();
            var jobs = await session.Query<JobEntity>().Where(j => j.isRetired == false).Select(job => new JobItemModel
            {
                Id = job.Slug,
                Title = job.Title,
                Description = job.Description
            }).ToListAsync();
            return new CollectionResponse<JobItemModel> { Data = jobs.ToList() };
        }

        public async Task<JobItemModel?> GetJobBySlugAsync(string slug)
        {
            using var session = _documentStore.LightweightSession();
            var job = await session.Query<JobEntity>()
                .Where(j => j.isRetired == false && j.Slug == slug)
                .Select(job => new JobItemModel
                {
                    Id = job.Slug,
                    Title = job.Title,
                    Description = job.Description
                }).SingleOrDefaultAsync();
            return job;
        }

        public async Task<bool> CheckForJobAsync(string slug)
        {
            using var session = _documentStore.LightweightSession();
            return await session.Query<JobEntity>()
                .Where(j => j.isRetired == false && j.Slug == slug)
                .AnyAsync();
        }
    }
}