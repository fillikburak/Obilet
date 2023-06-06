
using Obilet.Common;
using Obilet.Common.Models.Journey;

namespace Obilet.Infrastructure.Interfaces;
public interface IJourneyService
{
    Task<ServiceResponse<List<JourneyResponseModel>>> GetJourneys(JourneyRequestModel model);
}
