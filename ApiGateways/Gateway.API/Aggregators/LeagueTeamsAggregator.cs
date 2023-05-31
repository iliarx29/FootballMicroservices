using Gateway.API.Dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using System.Net;
using System.Net.Http.Headers;

namespace Gateway.API.Aggregators;

public class LeagueTeamsAggregator : IDefinedAggregator
{
    public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
    {
        var leagues = await responses[0].Items.DownstreamResponse().Content.ReadFromJsonAsync<List<LeagueDto>>();
        var teams = await responses[1].Items.DownstreamResponse().Content.ReadFromJsonAsync<List<TeamDto>>();

        leagues?.ForEach(league =>
        {
            league.Teams = teams.Where(x => x.LeagueId == league.Id).ToList();
        });

        var jsonString = JsonConvert.SerializeObject(leagues, Formatting.Indented, new JsonConverter[] { new StringEnumConverter() });

        var stringContent = new StringContent(jsonString)
        {
            Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
        };

        return new DownstreamResponse(stringContent, HttpStatusCode.OK, new List<KeyValuePair<string, IEnumerable<string>>>(), "OK");
    }
}
