
using Newtonsoft.Json;

namespace Obilet.Common.Models.Journey;
public class JourneyViewRequestModel
{
    [JsonProperty("originId")]
    public int? OriginId { get; set; }

    [JsonProperty("destinationId")]
    public int? DestinationId { get; set; }

    [JsonProperty("departureDate")]
    public string? DepartureDate { get; set; }
}
