using SlugGenerators;

namespace JobsApi.Controllers
{
    public class UniqueIdChecker : ICheckForUniqueValues
    {
        public Task<bool> IsUniqueAsync(string attempt)
        {
            return Task.FromResult(true);
        }
    }
}
