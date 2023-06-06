
using Newtonsoft.Json;

namespace Obilet.Common.Models.Base;

public class BaseRequestModel
{
    public BaseRequestModel()
    {
        DeviceSession = new DeviceSession();
    }

    [JsonProperty("device-session")]
    public DeviceSession? DeviceSession { get; set; }

    [JsonProperty("date")]
    public string? Date { get; set; }

    [JsonProperty("language")]
    public string? Language { get; set; }
}
