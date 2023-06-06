using Newtonsoft.Json;

namespace Obilet.Common.Models.Base;

[JsonObject("device-session")]
public class DeviceSession
{
    [JsonProperty("session-id")]
    public string? SessionId { get; set; }

    [JsonProperty("device-id")]
    public string? DeviceId { get; set; }
}
