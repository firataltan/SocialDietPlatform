using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialDietPlatform.Application.Interfaces.Services;

public interface ISearchService
{
    Task<IEnumerable<T>> SearchAsync<T>(string query, string indexName, int page, int pageSize, CancellationToken cancellationToken = default) where T : class;
    Task IndexDocumentAsync<T>(T document, string indexName, CancellationToken cancellationToken = default) where T : class;
    Task DeleteDocumentAsync(string id, string indexName, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetAutoCompleteAsync(string query, string indexName, string field, CancellationToken cancellationToken = default);
}