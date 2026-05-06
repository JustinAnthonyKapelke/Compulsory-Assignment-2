using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace AI_Research_Agent
{
    public class OpenAlexTool
    {
        private readonly HttpClient _http = new();

        public async Task<List<Paper>> Search(string query)
        {
            var encoded = Uri.EscapeDataString(query);
            var url = $"https://api.openalex.org/works?search={encoded}&per-page=50";

            var json = await _http.GetStringAsync(url);
            var obj = JObject.Parse(json);

            var results = new List<Paper>();

            var items = obj["results"] as JArray;

            if (items == null || !items.Any())
                return results;

            foreach (var item in items)
            {
                var authors = item["authorships"]?
                    .Select(a => a["author"]?["display_name"]?.ToString())
                    .Where(name => !string.IsNullOrWhiteSpace(name))
                    .ToList() ?? new List<string>();

                results.Add(new Paper
                {
                    Title = item["title"]?.ToString() ?? "Unknown title",
                    Year = (int?)item["publication_year"] ?? 0,
                    Citations = (int?)item["cited_by_count"] ?? 0,
                    Url = item["open_access"]?["oa_url"]?.ToString()
                          ?? item["id"]?.ToString()
                          ?? "",
                    Authors = string.Join(", ", authors)
                });
            }

            return results;
        }
    }
}