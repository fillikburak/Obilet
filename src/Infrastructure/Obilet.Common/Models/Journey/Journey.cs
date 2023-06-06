
using System.Text.Json.Serialization;

namespace Obilet.Common.Models.Journey;
public class Journey
{
    [JsonPropertyName("kind")]
    public string? Kind { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("stops")]
    public List<Stop>? Stops { get; set; }

    [JsonPropertyName("origin")]
    public string? Origin { get; set; }

    [JsonPropertyName("destination")]
    public string? Destination { get; set; }

    [JsonPropertyName("departure")]
    public DateTime? Departure { get; set; }

    public string? DepartureTime { get; set; }

    [JsonPropertyName("arrival")]
    public DateTime? Arrival { get; set; }

    public string? ArrivalTime { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("duration")]
    public string? Duration { get; set; }

    [JsonPropertyName("original-price")]
    public double? OriginalPrice { get; set; }

    [JsonPropertyName("internet-price")]
    public double? InternetPrice { get; set; }

    [JsonPropertyName("provider-internet-price")]
    public double? ProviderInternetPrice { get; set; }

    [JsonPropertyName("booking")]
    public object? Booking { get; set; }

    [JsonPropertyName("bus-name")]
    public string? Busname { get; set; }

    [JsonPropertyName("policy")]
    public Policy? Policy { get; set; }

    [JsonPropertyName("features")]
    public List<string>? Features { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("available")]
    public object? Available { get; set; }
}
