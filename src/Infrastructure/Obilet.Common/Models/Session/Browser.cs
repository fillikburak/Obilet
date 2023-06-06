using Newtonsoft.Json;

namespace Obilet.Common.Models.Session;

public class Browser
{
    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("version")]
    public string? Version { get; set; }
}
