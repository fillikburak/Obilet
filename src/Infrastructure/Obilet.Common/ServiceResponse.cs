using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Obilet.Common;
public class ServiceResponse
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("user-message")]
    public string? UserMessage { get; set; }

    [JsonPropertyName("api-request-id")]
    public Guid? ApiRequestId { get; set; }

    [JsonPropertyName("controller")]
    public string? Controller { get; set; }

    [JsonPropertyName("client-request-id")]
    public Guid? ClientRequestId { get; set; }

    [JsonPropertyName("web-correlation-id")]
    public Guid? WebCorrelationId { get; set; }

    [JsonPropertyName("correlation-id")]
    public Guid? CorrelationId { get; set; }

    public bool IsSuccess { get; set; }
}
