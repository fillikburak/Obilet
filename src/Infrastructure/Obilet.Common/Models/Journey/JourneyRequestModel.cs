
using Newtonsoft.Json;
using Obilet.Common.Models.Base;

namespace Obilet.Common.Models.Journey;
public class JourneyRequestModel : BaseRequestModel
{
    [JsonProperty("data")]
    public DataRequestModel? Data { get; set; }
}
