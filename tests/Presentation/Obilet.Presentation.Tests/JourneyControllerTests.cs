using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Obilet.Common.Models.Base;
using Obilet.Common.Models.Journey;
using Obilet.Common;
using Obilet.Infrastructure.Interfaces;
using ObiletWeb.Controllers;
using ObiletWeb.Models;

namespace Obilet.Presentation.Tests;
public class JourneyControllerTests
{
    private readonly JourneyController journeyController;
    private readonly Mock<IHttpContextAccessor> contextAccessorMock;
    private readonly Mock<Microsoft.Extensions.Configuration.IConfiguration> configurationMock;
    private readonly Mock<IJourneyService> journeyServiceMock;

    public JourneyControllerTests()
    {
        contextAccessorMock = new Mock<IHttpContextAccessor>();
        configurationMock = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
        journeyServiceMock = new Mock<IJourneyService>();

        journeyController = new JourneyController(
            contextAccessorMock.Object,
            configurationMock.Object,
            journeyServiceMock.Object
        );
    }

    [Fact]
    public async Task Journeys_ValidRequest_ReturnsJourneyView()
    {
        // Arrange
        var requestModel = new JourneyViewRequestModel
        {
            DepartureDate = "2023-06-07",
            DestinationId = 2,
            OriginId = 1
        };

        var journeyRequestModel = new JourneyRequestModel
        {
            Data = new DataRequestModel
            {
                DepartureDate = requestModel.DepartureDate,
                DestinationId = requestModel.DestinationId,
                OriginId = requestModel.OriginId
            },
            DeviceSession = new DeviceSession
            {
                SessionId = "sessionId",
                DeviceId = "deviceId"
            },
            Language = "Tr"
        };

        var journeys = new ServiceResponse<List<JourneyResponseModel>>
        {
            IsSuccess = true,
            Data = new List<JourneyResponseModel>
                {
                    new JourneyResponseModel
                    {
                        Journey = new Journey
                        {
                            Departure = new DateTime(2023, 06, 07, 10, 30, 0),
                            Arrival = new DateTime(2023, 06, 07, 14, 45, 0)
                        },
                        OriginLocation = "Origin Location",
                        DestinationLocation = "Destination Location"
                    }
                }
        };

        contextAccessorMock.Setup(c => c.HttpContext.Request.Cookies["SessionId"]).Returns("sessionId");
        contextAccessorMock.Setup(c => c.HttpContext.Request.Cookies["DeviceId"]).Returns("deviceId");
        configurationMock.Setup(c => c["Language:Tr"]).Returns("Tr");
        journeyServiceMock.Setup(s => s.GetJourneys(It.IsAny<JourneyRequestModel>())).ReturnsAsync(journeys);

        // Act
        var result = await journeyController.Journeys(requestModel);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Journey", viewResult.ViewName);

        var viewModel = Assert.IsType<JourneyViewModel>(viewResult.Model);
        Assert.Equal(journeys.Data.OrderBy(j => j.Journey.Departure).ToList(), viewModel.Journeys);
        Assert.Equal(requestModel.DepartureDate, viewModel.DepartureDate);
        Assert.Equal(journeys.Data.FirstOrDefault()?.OriginLocation, viewModel.OriginLocation);
        Assert.Equal(journeys.Data.FirstOrDefault()?.DestinationLocation, viewModel.DestinationLocation);
    }

    [Fact]
    public async Task Journeys_UnsuccessfulRequest_ReturnsErrorView()
    {
        // Arrange
        var requestModel = new JourneyViewRequestModel
        {
            DepartureDate = "2023-06-07",
            DestinationId = 2,
            OriginId = 1
        };

        var errorMessage = "An error occurred while retrieving journeys.";

        var journeys = new ServiceResponse<List<JourneyResponseModel>>
        {
            IsSuccess = false,
            Message = errorMessage
        };

        journeyServiceMock.Setup(s => s.GetJourneys(It.IsAny<JourneyRequestModel>())).ReturnsAsync(journeys);

        // Act
        var result = await journeyController.Journeys(requestModel);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Error", viewResult.ViewName);

        var errorViewModel = Assert.IsType<ErrorViewModel>(viewResult.Model);
        Assert.Equal(errorMessage, errorViewModel.Message);
    }
}
