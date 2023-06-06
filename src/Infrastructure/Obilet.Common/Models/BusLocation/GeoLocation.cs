
using System.Text.Json.Serialization;

namespace Obilet.Common.Models.BusLocation;

public class GeoLocation
{
    [JsonPropertyName("latitude")]
    public decimal Latitude { get; set; }

    [JsonPropertyName("longitude")]

    public decimal Longitude { get; set; }

    [JsonPropertyName("zoom")]
    public int Zoom { get; set; }
}
