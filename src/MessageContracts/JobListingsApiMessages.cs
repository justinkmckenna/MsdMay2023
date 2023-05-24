namespace MessageContracts
{

    public class JobListingCreated
    {
        public static readonly string MessageId = "JobListings.JobListingCreated";
        public string Id { get; set; } = "";
        public string JobId { get; set; } = "";
        public string JobName { get; set; } = "";
        public string OpeningStartDate { get; set; } = "";
        public Salaryrange SalaryRange { get; set; } = new();
    }

    public class Salaryrange
    {
        public decimal Min { get; set; }
        public decimal Max { get; set; }
    }

}
