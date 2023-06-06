
namespace Obilet.Common.Models.Journey;
public class JourneyViewModel
{
    public List<JourneyResponseModel>? Journeys { get; set; }
    public string? DepartureDate { get; set; }
    public string? OriginLocation { get; set; }
    public string? DestinationLocation { get; set; }
}
