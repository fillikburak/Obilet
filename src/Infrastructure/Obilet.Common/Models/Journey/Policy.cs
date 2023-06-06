
using System.Text.Json.Serialization;

namespace Obilet.Common.Models.Journey;
public class Policy
{
    [JsonPropertyName("max-seats")]
    public int? MaxSeats { get; set; }

    [JsonPropertyName("max-single")]
    public int? MaxSingle { get; set; }

    [JsonPropertyName("max-single-males")]
    public int? MaxSingleMales { get; set; }

    [JsonPropertyName("max-single-females")]
    public int? MaxSingleFemales { get; set; }

    [JsonPropertyName("mixed-genders")]
    public bool? MixedGenders { get; set; }

    [JsonPropertyName("gov-id")]
    public bool? GovId { get; set; }

    [JsonPropertyName("lht")]
    public bool? Lht { get; set; }
}
