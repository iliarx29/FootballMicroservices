using Matches.Application.Abstractions;
using Nest;

namespace Matches.Application.Services;
public class ElasticService : IElasticService
{
    private readonly IElasticClient _elasticClient;

    public ElasticService(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task<ISearchResponse<T>> SearchAsync<T>(Func<SearchDescriptor<T>, ISearchRequest> selector, CancellationToken cancellationToken = default) where T : class
    {
        var result = await _elasticClient.SearchAsync(selector, cancellationToken);

        return result;
    }

    public bool CheckIndexExists(string indexName)
    {
        var exists = _elasticClient.Indices.Exists(indexName).Exists;

        return exists;
    }

    public async Task<IndexResponse> AddDocument<T>(T document, CancellationToken cancellationToken = default)
        where T : class
    {
        var response = await _elasticClient.IndexDocumentAsync(document, cancellationToken);

        return response;
    }

    public async Task<BulkResponse> AddDocumentsToElastic<T>(List<T> documents, string indexName)
        where T : class
    {
        var response = await _elasticClient.IndexManyAsync(documents, indexName);

        return response;
    }

    public async Task<CreateIndexResponse> CreateIndex(string indexName, Func<CreateIndexDescriptor, ICreateIndexRequest> descriptor)
    {
        var response = await _elasticClient.Indices.CreateAsync(indexName, descriptor);

        return response;
    }

}
