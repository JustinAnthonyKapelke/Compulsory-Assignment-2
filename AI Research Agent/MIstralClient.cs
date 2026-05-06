using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AI_Research_Agent
{
    public class MistralClient
    {
        private readonly HttpClient _http = new();
        private readonly string _apiKey;

        public MistralClient(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<string> Ask(string prompt)
        {
            var body = new
            {
                model = "open-mistral-nemo",
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                temperature = 0
            };

            var req = new HttpRequestMessage(
                HttpMethod.Post,
                "https://api.mistral.ai/v1/chat/completions"
            );

            req.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);

            req.Content = new StringContent(
                JsonConvert.SerializeObject(body),
                Encoding.UTF8,
                "application/json"
            );

            var res = await _http.SendAsync(req);
            var raw = await res.Content.ReadAsStringAsync();

            var parsed = JsonConvert.DeserializeObject<dynamic>(raw);
            return parsed["choices"][0]["message"]["content"]?.ToString() ?? "No response";
        }
    }
}