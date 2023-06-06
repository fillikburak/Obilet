
using Newtonsoft.Json;

namespace Obilet.Common.Models.Journey;
public class DataRequestModel
{
    [JsonProperty("origin-id")]
    public int? OriginId { get; set; }

    [JsonProperty("destination-id")]
    public int? DestinationId { get; set; }

    [JsonProperty("departure-date")]
    public string? DepartureDate { get; set; }
}
