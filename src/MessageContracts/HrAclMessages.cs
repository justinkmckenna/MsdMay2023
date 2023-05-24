namespace MessageContracts
{
    public class HrAclMessages
    {
        public class HiringRequest
        {
            public static readonly string MessageId = "HrAcl.HiringRequestCreated";
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string EmailAddress { get; set; } = string.Empty;
            public string JobId { get; set; } = string.Empty;
            public string OfferingId { get; set; } = string.Empty;
        }

        public class EmployeeHired
        {
            public static readonly string MessageId = "Hr.EmployeeHired";
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string EmailAddress { get; set; } = string.Empty;
            public string JobId { get; set; } = string.Empty;
        }

        public class JobApplicationDenied // we don't really care about this, guess we can just ghost the applicants lol
        {
            public string OfferingId { get; set; } = "";
        }
    }
}
