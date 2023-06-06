
using System.Text.Json.Serialization;

namespace Obilet.Common.Models.Journey;
public class Feature
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("priority")]
    public int? Priority { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("is-promoted")]
    public bool? IsPromoted { get; set; }

    [JsonPropertyName("back-color")]
    public string? BackColor { get; set; }

    [JsonPropertyName("fore-color")]
    public string? ForeColor { get; set; }
}
