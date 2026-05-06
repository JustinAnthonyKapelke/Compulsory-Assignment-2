using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AI_Research_Agent
{
    public class AutoGenTool
    {
        private readonly HttpClient _http;

        public AutoGenTool(HttpClient http)
        {
            _http = http;
        }

        public async Task<string> Run(string query)
        {
            var payload = JsonSerializer.Serialize(new
            {
                title = query,
                year = 0,
                citations = 0
            });

            var response = await _http.PostAsync(
                "http://localhost:8000/research",
                new StringContent(payload, Encoding.UTF8, "application/json")
            );

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
