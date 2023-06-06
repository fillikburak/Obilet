

using Obilet.Common;
using Obilet.Common.Models.BusLocation;

namespace Obilet.Infrastructure.Interfaces;

public interface IBusLocationService
{
    Task<ServiceResponse<List<BusLocationResponseModel>>> GetBusLocations(BusLocationRequestModel model);
}
