
using Microsoft.Extensions.Configuration;
using Obilet.Common;
using Obilet.Common.Models.BusLocation;
using Obilet.Infrastructure.Interfaces;

namespace Obilet.Infrastructure.Services;
public class BusLocationService : IBusLocationService
{
    private readonly IServiceResponseHelper serviceResponse;
    private readonly IHttpClientService httpService;
    private readonly IConfiguration configuration;

    public BusLocationService(
        IServiceResponseHelper serviceResponse,
        IHttpClientService httpService,
        IConfiguration configuration)
    {
        this.serviceResponse = serviceResponse;
        this.httpService = httpService;
        this.configuration = configuration;
    }

    public async Task<ServiceResponse<List<BusLocationResponseModel>>> GetBusLocations(BusLocationRequestModel model)
    {
        return await httpService.PostApiRequestAsync<List<BusLocationResponseModel>>(configuration["Obilet:GetBusLocations"], model);
    }
}
