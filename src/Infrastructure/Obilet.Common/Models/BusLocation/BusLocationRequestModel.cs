
using Newtonsoft.Json;
using Obilet.Common.Models.Base;

namespace Obilet.Common.Models.BusLocation;
public class BusLocationRequestModel : BaseRequestModel
{
    [JsonProperty("data")]
    public string? Data { get; set; }
}
