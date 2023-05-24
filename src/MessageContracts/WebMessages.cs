﻿namespace MessageContracts
{
    public class ApplicantCreated
    {
        public static readonly string MessageId = "WebMessages.ApplicantCreated";
        public string UserId { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public DateTimeOffset DateOfBirth { get; set; }
        public string EmailAddress { get; set; } = "";
    }
}
