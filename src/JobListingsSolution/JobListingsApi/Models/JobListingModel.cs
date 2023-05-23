using System.ComponentModel.DataAnnotations;

namespace JobListingsApi.Models
{
    public class JobListingCreateModel
    {
        [Required]
        public string OpeningStartDate { get; set; } = "";
        [Required]
        public SalaryRangeModel SalaryRange { get; set; } = new();
    }

    public class JobListingEntity : JobListingCreateModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string JobId { get; set; } = "";
        public string JobName { get; set; } = "";
    }

    public class SalaryRangeModel
    {
        [Required]
        public decimal? Min { get; set; }
        [Required]
        public decimal? Max { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Max < Min)
            {
                yield return new ValidationResult("Max has to equal or be greater than Min");
            }
        }

    }
}
