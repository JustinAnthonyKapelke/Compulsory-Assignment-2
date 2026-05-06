using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace AI_Research_Agent
{
    public class ResearchAgent
    {
        private readonly HttpClient _httpClient;
        private readonly OpenAlexTool _tool = new();

        public ResearchAgent(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> Run(string userQuery, Constraints constraints = null)
        {
            var papers = await _tool.Search(userQuery);

            if (papers == null || !papers.Any())
                return "No papers found";

            var payload = new
            {
                query = userQuery,
                constraints = constraints,
                papers = papers.Select(p => new
                {
                    title = p.Title,
                    year = p.Year,
                    citations = p.Citations,
                    authors = p.Authors,
                    url = p.Url
                }).ToList()
            };

            var json = JsonSerializer.Serialize(payload);

            try
            {
                var response = await _httpClient.PostAsync(
                    "http://localhost:8000/research",
                    new StringContent(json, Encoding.UTF8, "application/json")
                );

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                return $"Failed to contact research service: {ex.Message}";
            }
        }
    }
}