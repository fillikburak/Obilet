

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Obilet.Common.Models.BusLocation;
using Obilet.Common.Models.Session;
using Obilet.Common;
using Obilet.Infrastructure.Interfaces;
using ObiletWeb.Controllers;
using ObiletWeb.Models;
using System.Text.Json;
using System.Net;

namespace Obilet.Presentation.Tests;
public class HomeControllerTests
{
    private readonly Mock<IHttpContextAccessor> contextAccessorMock;
    private readonly Mock<IHttpClientService> httpServiceMock;
    private readonly Mock<Microsoft.Extensions.Configuration.IConfiguration> configurationMock;
    private readonly Mock<IBusLocationService> busLocationServiceMock;
    private readonly Mock<IDistributedCache> distributedCacheMock;
    private readonly Mock<IServiceResponseHelper> serviceResponseHelperMock;
    private readonly HomeController homeController;

    public HomeControllerTests()
    {
        contextAccessorMock = new Mock<IHttpContextAccessor>();
        httpServiceMock = new Mock<IHttpClientService>();
        configurationMock = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
        busLocationServiceMock = new Mock<IBusLocationService>();
        distributedCacheMock = new Mock<IDistributedCache>();
        serviceResponseHelperMock = new Mock<IServiceResponseHelper>();

        homeController = new HomeController(
            contextAccessorMock.Object,
            httpServiceMock.Object,
            configurationMock.Object,
            busLocationServiceMock.Object,
            distributedCacheMock.Object,
            serviceResponseHelperMock.Object
        );
    }

    [Fact]
    public async Task Index_ReturnsViewResult_WithBusLocations()
    {
        // Arrange
        var cookie = new ServiceResponse<SessionResponseModel>
        {
            IsSuccess = true,
            Data = new SessionResponseModel
            {
                SessionId = "sessionId",
                DeviceId = "deviceId"
            }
        };

        var busLocations = new ServiceResponse<List<BusLocationResponseModel>>
        {
            IsSuccess = true,
            Data = new List<BusLocationResponseModel>
                {
                    new BusLocationResponseModel { CountryName = "Türkiye" },
                    new BusLocationResponseModel { CountryName = "Kuzey Kıbrıs Türk Cumhuriyeti" }
                }
        };

        contextAccessorMock.Setup(c => c.HttpContext.Request.Cookies["SessionId"]).Returns(cookie.Data.SessionId);
        contextAccessorMock.Setup(c => c.HttpContext.Request.Cookies["DeviceId"]).Returns(cookie.Data.DeviceId);

        httpServiceMock.Setup(s => s.PostApiRequestAsync<SessionResponseModel>(It.IsAny<string>(), It.IsAny<SessionRequestModel>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(new ServiceResponse<SessionResponseModel> { IsSuccess = true, Data = cookie.Data });

        busLocationServiceMock.Setup(s => s.GetBusLocations(It.IsAny<BusLocationRequestModel>()))
            .ReturnsAsync(busLocations);

        serviceResponseHelperMock.Setup(s => s.SetSuccess(It.IsAny<SessionResponseModel>())).Returns(cookie);

        configurationMock.Setup(c => c["BusLocationCacheKey"]).Returns("BusLocationCacheKey");

        // Set up the behavior for Get method
        distributedCacheMock.Setup(c => c.Get(It.IsAny<string>()))
            .Returns((string cacheKey) =>
            {
                if (cacheKey == "BusLocationCacheKey")
                {
                    // Return a mock cache data as byte array
                    var cacheData = JsonSerializer.SerializeToUtf8Bytes(
                        new ServiceResponse<List<BusLocationResponseModel>>
                        {
                            IsSuccess = true,
                            Data = new List<BusLocationResponseModel>
                            {
                                new BusLocationResponseModel { CountryName = "Türkiye" },
                                new BusLocationResponseModel { CountryName = "Kuzey Kıbrıs Türk Cumhuriyeti" }
                            }
                        }
                    );

                    return cacheData;
                }

                return null; // Cache key not found
            });

        // Set up the behavior for Set method
        distributedCacheMock.Setup(c => c.Set(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>()));


        // Act
        var result = await homeController.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.IsType<List<BusLocationResponseModel>>(viewResult.Model);

        var model = Assert.IsAssignableFrom<List<BusLocationResponseModel>>(viewResult.Model);
        Assert.Equal(2, model.Count);
        Assert.Equal("Türkiye", model[0].CountryName);
        Assert.Equal("Kuzey Kıbrıs Türk Cumhuriyeti", model[1].CountryName);
    }

    [Fact]
    public async Task Index_ReturnsErrorView_WhenCookieIsNotSet()
    {
        // Arrange
        var errorResponse = new ServiceResponse<SessionResponseModel>
        {
            IsSuccess = false,
            Message = "Cookie not set error"
        };

        var remoteIpAddress = IPAddress.Parse("192.168.1.1");
        var remotePort = 12345;
        var browserVersion = "\v=\'1";

        contextAccessorMock.Setup(c => c.HttpContext.Request.Cookies["IpAddress"]).Returns("0.0.0.1");
        contextAccessorMock.Setup(c => c.HttpContext.Request.Cookies["Port"]).Returns(It.IsAny<string>());
        contextAccessorMock.Setup(c => c.HttpContext.Request.Headers["sec-ch-ua"]).Returns(browserVersion);
        contextAccessorMock.Setup(c => c.HttpContext.Connection.RemoteIpAddress).Returns(remoteIpAddress);
        contextAccessorMock.Setup(c => c.HttpContext.Connection.RemotePort).Returns(remotePort);

        httpServiceMock.Setup(s => s.PostApiRequestAsync<SessionResponseModel>(It.IsAny<string>(), It.IsAny<SessionRequestModel>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(errorResponse);

        serviceResponseHelperMock.Setup(s => s.SetError(It.IsAny<SessionResponseModel>(), It.IsAny<string>(), It.IsAny<string>())).Returns(errorResponse);

        // Act
        var result = await homeController.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Error", viewResult.ViewName);

        var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
        Assert.Equal("Cookie not set error", model.Message);
    }

    [Fact]
    public async Task Index_ReturnsErrorView_WhenBusLocationsFetchFails()
    {
        // Arrange
        var cookie = new ServiceResponse<SessionResponseModel>
        {
            IsSuccess = true,
            Data = new SessionResponseModel
            {
                SessionId = "sessionId",
                DeviceId = "deviceId"
            }
        };

        var errorResponse = new ServiceResponse<List<BusLocationResponseModel>>
        {
            IsSuccess = false,
            Message = "Bus locations fetch error"
        };

        var remoteIpAddress = IPAddress.Parse("192.168.1.1");
        var remotePort = 12345;
        var browserVersion = "\v=\'1";

        contextAccessorMock.Setup(c => c.HttpContext.Request.Cookies["IpAddress"]).Returns("0.0.0.1");
        contextAccessorMock.Setup(c => c.HttpContext.Request.Cookies["Port"]).Returns(It.IsAny<string>());
        contextAccessorMock.Setup(c => c.HttpContext.Request.Headers["sec-ch-ua"]).Returns(browserVersion);
        contextAccessorMock.Setup(c => c.HttpContext.Connection.RemoteIpAddress).Returns(remoteIpAddress);
        contextAccessorMock.Setup(c => c.HttpContext.Connection.RemotePort).Returns(remotePort);

        contextAccessorMock.Setup(s => s.HttpContext.Response.Cookies.Append(It.IsAny<string>(), It.IsAny<string>())).Callback((string key, string value) =>
        {
            // Store the cookie values in a dictionary or other data structure for verification
            // You can use a Dictionary<string, string> to store the cookies for later validation
            // For example:
            // cookieDictionary.Add(key, value);
        });

        httpServiceMock.Setup(s => s.PostApiRequestAsync<SessionResponseModel>(It.IsAny<string>(), It.IsAny<SessionRequestModel>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(new ServiceResponse<SessionResponseModel> { IsSuccess = true, Data = cookie.Data });

        serviceResponseHelperMock.Setup(s => s.SetSuccess(It.IsAny<SessionResponseModel>())).Returns(cookie);

        busLocationServiceMock.Setup(s => s.GetBusLocations(It.IsAny<BusLocationRequestModel>()))
            .ReturnsAsync(errorResponse);

        serviceResponseHelperMock.Setup(s => s.SetError(It.IsAny<List<BusLocationResponseModel>>(), It.IsAny<string>(), It.IsAny<string>())).Returns(errorResponse);

        // Set up the behavior for Get method
        distributedCacheMock.Setup(c => c.Get(It.IsAny<string>()))
            .Returns((string cacheKey) =>
            {
                if (cacheKey == "BusLocationCacheKey")
                {
                    // Return a mock cache data as byte array
                    var cacheData = JsonSerializer.SerializeToUtf8Bytes(
                        new ServiceResponse<List<BusLocationResponseModel>>
                        {
                            IsSuccess = true,
                            Data = new List<BusLocationResponseModel>
                            {
                                new BusLocationResponseModel { CountryName = "Türkiye" },
                                new BusLocationResponseModel { CountryName = "Kuzey Kıbrıs Türk Cumhuriyeti" }
                            }
                        }
                    );

                    return cacheData;
                }

                return null; // Cache key not found
            });

        // Set up the behavior for Set method
        distributedCacheMock.Setup(c => c.Set(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>()));

        // Act
        var result = await homeController.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Error", viewResult.ViewName);

        var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
        Assert.Equal("Bus locations fetch error", model.Message);
    }
}
