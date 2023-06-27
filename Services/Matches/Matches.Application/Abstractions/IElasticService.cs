using Nest;

namespace Matches.Application.Abstractions;
public interface IElasticService
{
    Task<ISearchResponse<T>> SearchAsync<T>(
        Func<SearchDescriptor<T>, ISearchRequest> selector,
        CancellationToken cancellationToken = default) where T : class;

    bool CheckIndexExists(string indexName);

    Task<IndexResponse> AddDocument<T>(T document, CancellationToken cancellationToken = default)
        where T : class;

    Task<BulkResponse> AddDocumentsToElastic<T>(List<T> documents, string indexName)
        where T : class;

    Task<CreateIndexResponse> CreateIndex(string indexName, Func<CreateIndexDescriptor, ICreateIndexRequest> descriptor);
}
