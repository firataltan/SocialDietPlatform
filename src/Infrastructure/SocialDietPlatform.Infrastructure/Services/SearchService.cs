using Nest;
using SocialDietPlatform.Application.Interfaces.Services;

namespace SocialDietPlatform.Infrastructure.Services;

public class SearchService : ISearchService
{
    private readonly IElasticClient _elasticClient;

    public SearchService(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task<IEnumerable<T>> SearchAsync<T>(string query, string indexName, int page, int pageSize, CancellationToken cancellationToken = default) where T : class
    {
        var searchResponse = await _elasticClient.SearchAsync<T>(s => s
            .Index(indexName)
            .Query(q => q
                .MultiMatch(mm => mm
                    .Fields(f => f
                        .Field("*")
                    )
                    .Query(query)
                    .Fuzziness(Fuzziness.Auto)
                )
            )
            .From((page - 1) * pageSize)
            .Size(pageSize),
            cancellationToken
        );

        return searchResponse.Documents;
    }

    public async Task IndexDocumentAsync<T>(T document, string indexName, CancellationToken cancellationToken = default) where T : class
    {
        await _elasticClient.IndexDocumentAsync(document, cancellationToken);
    }

    public async Task DeleteDocumentAsync(string id, string indexName, CancellationToken cancellationToken = default)
    {
        await _elasticClient.DeleteAsync<object>(id, d => d.Index(indexName), cancellationToken);
    }

    public async Task<IEnumerable<string>> GetAutoCompleteAsync(string query, string indexName, string field, CancellationToken cancellationToken = default)
    {
        var searchResponse = await _elasticClient.SearchAsync<object>(s => s
            .Index(indexName)
            .Suggest(su => su
                .Completion("suggestions", c => c
                    .Field(field)
                    .Prefix(query)
                    .Fuzzy(f => f
                        .Fuzziness(Fuzziness.Auto)
                    )
                    .Size(10)
                )
            ),
            cancellationToken
        );

        var suggestions = searchResponse.Suggest["suggestions"]
            .SelectMany(s => s.Options)
            .Select(o => o.Text);

        return suggestions;
    }
} 