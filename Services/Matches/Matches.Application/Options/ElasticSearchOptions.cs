namespace Matches.Application.Options;
internal class ElasticSearchOptions
{
    public const string ElasticSearch = "ElasticSearch";

    public string Connection { get; set; } = string.Empty;
    public string IndexName { get; set; } = string.Empty;
}
