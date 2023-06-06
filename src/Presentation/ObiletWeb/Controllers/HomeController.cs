using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Obilet.Common;
using Obilet.Common.Models.Base;
using Obilet.Common.Models.BusLocation;
using Obilet.Common.Models.Session;
using Obilet.Infrastructure.Interfaces;
using ObiletWeb.Models;
using System.Text.Json;

namespace ObiletWeb.Controllers;
public class HomeController : Controller
{
    #region Constructor

    private readonly IHttpContextAccessor context;
    private readonly IHttpClientService httpService;
    private readonly IConfiguration configuration;
    private readonly IBusLocationService busLocationService;
    private readonly IDistributedCache distributedCache;
    private readonly IServiceResponseHelper serviceResponse;

    public HomeController(
        IHttpContextAccessor context,
        IHttpClientService httpService,
        IConfiguration configuration,
        IBusLocationService busLocationService,
        IDistributedCache distributedCache,
        IServiceResponseHelper serviceResponse)
    {
        this.context = context;
        this.httpService = httpService;
        this.configuration = configuration;
        this.busLocationService = busLocationService;
        this.distributedCache = distributedCache;
        this.serviceResponse = serviceResponse;
    }

    #endregion

    #region Views

    public async Task<IActionResult> Index()
    {
        var cookie = await SetCookies();

        if (!cookie.IsSuccess)
        {
            return View("Error", new ErrorViewModel { Message = cookie.Message });
        }

        var locations = await GetBusLocations(cookie.Data);

        if (!locations.IsSuccess)
        {
            return View("Error", new ErrorViewModel { Message = locations.Message });
        }

        return View(locations.Data);
    }

    #endregion

    #region Functions

    private async Task<ServiceResponse<List<BusLocationResponseModel>>> GetBusLocations(SessionResponseModel cookie)
    {
        #region Generate Bus Location Request Model
        var model = new BusLocationRequestModel
        {
            DeviceSession = new DeviceSession
            {
                SessionId = cookie.SessionId,
                DeviceId = cookie.DeviceId
            },
            Language = configuration["Language:Tr"]
        };
        #endregion

        //Check cache and return if its not empty
        byte[] cacheData = distributedCache.Get(configuration["BusLocationCacheKey"]);
        if (cacheData != null)
        {
            return JsonSerializer.Deserialize<ServiceResponse<List<BusLocationResponseModel>>>(cacheData);
        }

        //get bus locations and set to cache
        var busLocations = await FetchBusLocationsFromDataSource(model);

        if (busLocations.IsSuccess)
        {
            cacheData = JsonSerializer.SerializeToUtf8Bytes(busLocations);
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            distributedCache.Set(configuration["BusLocationCacheKey"], cacheData, cacheOptions);

            return busLocations;
        }

        return busLocations;
    }

    //Get bus locations from api if cache is empty
    private async Task<ServiceResponse<List<BusLocationResponseModel>>> FetchBusLocationsFromDataSource(BusLocationRequestModel model)
    {
        var busLocations = await busLocationService.GetBusLocations(model);

        if (busLocations.IsSuccess)
        {
            return serviceResponse.SetSuccess(busLocations.Data);
        }

        return serviceResponse.SetError(busLocations.Data, busLocations.Message, busLocations.UserMessage);
    }

    private async Task<ServiceResponse<SessionResponseModel>> SetCookies()
    {
        //if cookies are not set
        if (string.IsNullOrEmpty(context?.HttpContext?.Request.Cookies["SessionId"]) || string.IsNullOrEmpty(context?.HttpContext?.Request.Cookies["DeviceId"]))
        {
            #region Generate Session Request Model
            Random random = new Random();

            string ipAddress = context?.HttpContext.Connection.RemoteIpAddress.MapToIPv4()?.ToString();
            string port = context?.HttpContext.Connection.RemotePort.ToString();
            string browserName = GetBrowserName();
            string browserVersion = GetBrowserVersion();

            //set random ip number when running on local machine to get sessionId and deviceId
            if (ipAddress == "0.0.0.1")
                ipAddress = $"{random.Next(1, 256)}.{random.Next(0, 256)}.{random.Next(0, 256)}.{random.Next(1, 256)}";

            var request = new SessionRequestModel
            {
                Type = 1,
                Connection = new Connection
                {
                    IpAddress = ipAddress,
                    Port = port
                },
                Browser = new Browser
                {
                    Name = browserName,
                    Version = browserVersion
                }
            };
            #endregion

            var response = await httpService.PostApiRequestAsync<SessionResponseModel>(configuration["Obilet:GetSessionUrl"], request);

            if (response.IsSuccess)
            {
                context?.HttpContext.Response.Cookies.Append("SessionId", response.Data.SessionId);
                context?.HttpContext.Response.Cookies.Append("DeviceId", response.Data.DeviceId);

                ViewBag.UserIp = ipAddress;
                ViewBag.UserBrowser = browserName;

                return serviceResponse.SetSuccess(response.Data);
            }
            else
            {
                return serviceResponse.SetError(response.Data, response.Message, response.UserMessage);
            }
        }

        //if cookies are already set
        var session = new SessionResponseModel
        {
            SessionId = context?.HttpContext?.Request.Cookies["SessionId"],
            DeviceId = context?.HttpContext?.Request.Cookies["DeviceId"]
        };

        return serviceResponse.SetSuccess(session);
    }

    private string GetBrowserName()
    {
        string userAgent = context.HttpContext.Request.Headers["sec-ch-ua"].ToString();

        var browserMappings = new Dictionary<string, string>
        {
            { "MSIE", "Internet Explorer" },
            { "Chrome", "Google Chrome" },
            { "Firefox", "Mozilla Firefox" },
            { "Safari", "Apple Safari" },
            { "Opera", "Opera" },
            { "Edge", "Microsoft Edge" },
            { "Brave", "Brave" }
        };

        var matchedMapping = browserMappings.FirstOrDefault(mapping => userAgent.Contains(mapping.Key));

        return matchedMapping.Value ?? "Unknown";
    }

    private string GetBrowserVersion()
    {
        string userAgent = context.HttpContext.Request.Headers["sec-ch-ua"].ToString();

        int startIndex = userAgent.IndexOf("v=\"") + 3;
        int endIndex = userAgent.IndexOf("\"", startIndex);

        if (startIndex != -1 && endIndex != -1)
        {
            return userAgent.Substring(startIndex, endIndex - startIndex);
        }

        return string.Empty;
    }

    #endregion
}
