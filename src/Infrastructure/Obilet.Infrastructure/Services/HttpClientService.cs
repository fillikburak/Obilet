
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Obilet.Common;
using Obilet.Infrastructure.Interfaces;
using System.Net.Http.Json;
using System.Text;

namespace Obilet.Infrastructure.Services;
public class HttpClientService : IHttpClientService
{
    private readonly HttpClient httpClient;
    private readonly IServiceResponseHelper serviceResponse;
    private readonly IConfiguration configuration;

    public HttpClientService(HttpClient httpClient, IServiceResponseHelper serviceResponse, IConfiguration configuration)
    {
        this.httpClient = httpClient;
        this.serviceResponse = serviceResponse;
        this.configuration = configuration;
    }

    public async Task<ServiceResponse<T>> PostApiRequestAsync<T>(string url, object? body = null, bool addAuthorization = true, bool isLogging = true)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url);

        if (addAuthorization)
        {
            request.Headers.Add("Authorization", configuration["Token"]);
        }

        if (body != null)
        {
            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
        }

        var response = await httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<T>>();

            if (data is null)
            {
                return serviceResponse.SetError<T>(data?.Message ?? "Post işlemi sırasında bir hata oluştu");
            }

            if (data.Data is null)
            {
                return serviceResponse.SetError<T>(data?.Message ?? "Post işlemi sırasında bir hata oluştu");
            }

            return serviceResponse.SetSuccess<T>(data.Data);
        }
        else
        {
            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<T>>();

            return serviceResponse.SetError<T>(data?.Message ?? "Post işlemi sırasında bir hata oluştu");
        }
    }
}
