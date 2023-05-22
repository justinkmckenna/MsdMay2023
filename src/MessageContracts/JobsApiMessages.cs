namespace MessageContracts.JobsApi
{
    public class JobCreated
    {
        public static readonly string MessageId = "JobsApi.JobCreated";
        public string Id { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
    }

    public class JobRetired
    {
        public static readonly string MessageId = "JobsApi.JobRetired";
        public string Id { get; set; } = "";
    }
}
