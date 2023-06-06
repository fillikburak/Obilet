
using Microsoft.Extensions.Configuration;
using Obilet.Common;
using Obilet.Common.Models.BusLocation;
using Obilet.Common.Models.Journey;
using Obilet.Infrastructure.Interfaces;

namespace Obilet.Infrastructure.Services;
public class JourneyService : IJourneyService
{
    private readonly IServiceResponseHelper serviceResponse;
    private readonly IHttpClientService httpService;
    private readonly IConfiguration configuration;

    public JourneyService(
        IServiceResponseHelper serviceResponse, 
        IHttpClientService httpService, 
        IConfiguration configuration)
    {
        this.serviceResponse = serviceResponse;
        this.httpService = httpService;
        this.configuration = configuration;
    }

    public async Task<ServiceResponse<List<JourneyResponseModel>>> GetJourneys(JourneyRequestModel model)
    {
        return await httpService.PostApiRequestAsync<List<JourneyResponseModel>>(configuration["Obilet:GetJourneys"], model);
    }
}
