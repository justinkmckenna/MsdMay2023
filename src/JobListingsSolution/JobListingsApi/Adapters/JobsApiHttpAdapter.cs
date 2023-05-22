namespace JobListingsApi.Adapters
{
    public class JobsApiHttpAdapter
    {
        private readonly HttpClient _httpClient;

        public JobsApiHttpAdapter(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> JobExistsAsync(string jobId)
        {
            var response = await _httpClient.GetAsync($"/jobs/{jobId}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            } else
            {
                return true;
            }
        }
    }
}
