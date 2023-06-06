using Microsoft.AspNetCore.Mvc;
using Obilet.Common;
using Obilet.Common.Models.Base;
using Obilet.Common.Models.Journey;
using Obilet.Infrastructure.Interfaces;
using ObiletWeb.Models;
using System.Net;

namespace ObiletWeb.Controllers;
public class JourneyController : Controller
{
    private readonly IHttpContextAccessor context;
    private readonly IConfiguration configuration;
    private readonly IJourneyService journeyService;

    public JourneyController(
        IHttpContextAccessor context,
        IConfiguration configuration,
        IJourneyService journeyService)
    {
        this.context = context;
        this.configuration = configuration;
        this.journeyService = journeyService;
    }

    [HttpPost]
    public async Task<IActionResult> Journeys(JourneyViewRequestModel requestModel)
    {
        #region Generate Journey Request Model
        var model = new JourneyRequestModel
        {
            Data = new DataRequestModel
            {
                DepartureDate = requestModel.DepartureDate,
                DestinationId = requestModel.DestinationId,
                OriginId = requestModel.OriginId
            },
            DeviceSession = new DeviceSession
            {
                SessionId = context?.HttpContext?.Request.Cookies["SessionId"].ToString(),
                DeviceId = context?.HttpContext?.Request.Cookies["DeviceId"].ToString()
            },
            Language = configuration["Language:Tr"]
        }; 
        #endregion

        var journeys = await GetJourneys(model);

        if (!journeys.IsSuccess)
        {
            return View("Error", new ErrorViewModel { Message = journeys.Message });
        }

        SetDepartureTime(journeys);

        var viewModel = GenerateViewModel(journeys, requestModel.DepartureDate);

        return View("Journey", viewModel);
    }

    private async Task<ServiceResponse<List<JourneyResponseModel>>> GetJourneys(JourneyRequestModel model)
    {
        return await journeyService.GetJourneys(model);
    }

    //get departure time to show people the bus departure time instead of datetime
    private void SetDepartureTime(ServiceResponse<List<JourneyResponseModel>> journeys)
    {
        foreach (var journey in journeys.Data)
        {
            journey.Journey.DepartureTime = journey.Journey.Departure?.ToString("HH:mm");
            journey.Journey.ArrivalTime = journey.Journey.Arrival?.ToString("HH:mm");
        }
    }

    private JourneyViewModel GenerateViewModel(ServiceResponse<List<JourneyResponseModel>> journeys, string departureDate)
    {
        return new JourneyViewModel
        {
            Journeys = journeys.Data.OrderBy(journey => journey.Journey.Departure).ToList(),
            DepartureDate = departureDate,
            OriginLocation = journeys.Data.FirstOrDefault()?.OriginLocation,
            DestinationLocation = journeys.Data.FirstOrDefault()?.DestinationLocation
        };
    }
}
