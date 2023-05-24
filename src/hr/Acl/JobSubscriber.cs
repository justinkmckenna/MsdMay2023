using DotNetCore.CAP;
using Marten;
using MessageContracts;
using static MessageContracts.HrAclMessages;

namespace Acl;



public class DataSubscribers : ICapSubscribe
{



    private readonly IDocumentSession _documentSession;
    private readonly ILogger<DataSubscribers> _logger;
    private readonly ICapPublisher _publisher;


    public DataSubscribers(IDocumentSession documentSession, ILogger<DataSubscribers> logger, ICapPublisher publisher)
    {
        _documentSession = documentSession;
        _logger = logger;
        _publisher = publisher;
    }



    [CapSubscribe("JobsApi.JobCreated")]
    public async Task SaveJobAsync(MessageContracts.JobsApi.JobCreated jobCreated)
    {
        _logger.LogInformation($"Got a JobCreated {jobCreated.Title}");
        _documentSession.Store(jobCreated);
        await _documentSession.SaveChangesAsync();
    }



    [CapSubscribe("JobListings.JobListingCreated")]
    public async Task SaveJobOfferAsync(JobListingCreated offer)
    {
        _logger.LogInformation($"Got a JobListingCreated {offer.JobName}");
        _documentSession.Store(offer);
        await _documentSession.SaveChangesAsync();
    }



    [CapSubscribe("WebMessages.ApplicantCreated")]
    public async Task SaveApplicantAsync(ApplicantCreated applicant)
    {
        _logger.LogInformation($"Got a ApplicantCreated {applicant.EmailAddress}");
        _documentSession.Store(applicant);
        await _documentSession.SaveChangesAsync();
    }


    [CapSubscribe("WebMessages.JobApplicationCreated")]
    public async Task TurnApplicationIntoHiringRequestAsync(JobApplicationCreated application)
    {
        // we need the applicant.
        var applicant = await _documentSession
            .Query<ApplicantCreated>()
            .Where(a => a.UserId == application.ApplicantId).SingleAsync();
        //var offer = await _documentSession
        //    .Query<JobListingCreated>()
        //    .Where(o => o.Id == application.JobOfferingId).SingleAsync(); this should work but jobofferingid is messed up for some reason from mvc frontend, mocking below so I can follow still
        var offer = await _documentSession
           .Query<JobListingCreated>()
           .Where(o => true).FirstAsync();

        var messageToPublish = new HiringRequest
        {
            EmailAddress = applicant.EmailAddress,
            FirstName = applicant.FirstName,
            LastName = applicant.LastName,
            JobId = offer.JobId,
            OfferingId = application.JobOfferingId
        };

        await _publisher.PublishAsync(HiringRequest.MessageId, messageToPublish);
    }

    // todo create sub for closing the job offering when employee is hired
}