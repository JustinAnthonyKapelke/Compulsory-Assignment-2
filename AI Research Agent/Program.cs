using AI_Research_Agent;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;

var agent = new ResearchAgent(new HttpClient());

// ----------------------------
// INPUT
// ----------------------------
Console.Write("Enter query: ");
var query = Console.ReadLine() ?? "";

Console.Write("Min year (leave blank for default 2022): ");
var minYearInput = Console.ReadLine();

Console.Write("Max year (leave blank for default 2022): ");
var maxYearInput = Console.ReadLine();

Console.Write("Min citations (leave blank for default 100): ");
var minCitationsInput = Console.ReadLine();

var constraints = new Constraints
{
    MinYear = int.TryParse(minYearInput, out var yMin) ? yMin : null,
    MaxYear = int.TryParse(maxYearInput, out var yMax) ? yMax : null,
    MinCitations = int.TryParse(minCitationsInput, out var c) ? c : null
};

// ----------------------------
// CALL AGENT
// ----------------------------
var raw = await agent.Run(query, constraints);

// ----------------------------
// ERROR HANDLING
// ----------------------------
if (raw is "No papers found" or "No relevant papers found")
{
    Console.WriteLine(raw);
    return;
}

if (raw.StartsWith("Failed to contact research service:"))
{
    Console.WriteLine($"[Server Error] {raw}");
    return;
}

// ----------------------------
// FIX DOUBLE ENCODING 🔥
// ----------------------------
string cleaned = raw;

// hvis JSON er pakket ind i quotes → unwrap
if (!string.IsNullOrEmpty(cleaned) &&
    cleaned.StartsWith("\"") &&
    cleaned.EndsWith("\""))
{
    cleaned = JsonConvert.DeserializeObject<string>(cleaned);
}

// ----------------------------
// PARSE JSON
// ----------------------------
JObject data;
try
{
    data = JObject.Parse(cleaned);
}
catch (Exception ex)
{
    Console.WriteLine("[JSON PARSE ERROR]");
    Console.WriteLine(ex.Message);
    Console.WriteLine("\nRAW OUTPUT:");
    Console.WriteLine(raw);
    return;
}

// ----------------------------
// PRETTY OUTPUT
// ----------------------------
Console.WriteLine();
Console.WriteLine("====================================");
Console.WriteLine("        📚 RESEARCH RESULT");
Console.WriteLine("====================================");

Console.WriteLine($"📌 Title     : {data["title"] ?? "N/A"}");
Console.WriteLine($"👥 Authors   : {data["authors"] ?? "N/A"}");
Console.WriteLine($"📅 Year      : {data["year"] ?? "0"}");
Console.WriteLine($"📊 Citations : {data["citations"] ?? "0"}");
Console.WriteLine($"🔗 URL       : {data["url"] ?? "N/A"}");

Console.WriteLine();
Console.WriteLine("🧠 Summary:");
Console.WriteLine(data["summary"] ?? "No summary available");

Console.WriteLine("====================================");