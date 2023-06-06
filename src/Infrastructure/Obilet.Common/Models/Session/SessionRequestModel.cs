using Newtonsoft.Json;

namespace Obilet.Common.Models.Session;

public class SessionRequestModel
{
    [JsonProperty("type")]
    public int Type { get; set; }

    [JsonProperty("connection")]
    public Connection? Connection { get; set; }

    [JsonProperty("browser")]
    public Browser? Browser { get; set; }
}
