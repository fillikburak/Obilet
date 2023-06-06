
using Obilet.Common;

namespace Obilet.Infrastructure.Interfaces;

public interface IHttpClientService
{
    Task<ServiceResponse<T>> PostApiRequestAsync<T>(string url, object? body = null, bool addAuthorization = true, bool isLogging = true);
}
