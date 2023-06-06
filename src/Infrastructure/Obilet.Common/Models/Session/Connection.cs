using Newtonsoft.Json;

namespace Obilet.Common.Models.Session;

public class Connection
{
    [JsonProperty("ip-address")]
    public string? IpAddress { get; set; }

    [JsonProperty("port")]
    public string? Port { get; set; }
}
