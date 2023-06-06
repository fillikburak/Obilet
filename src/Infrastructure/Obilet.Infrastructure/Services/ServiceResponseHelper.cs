

using Obilet.Common;
using Obilet.Infrastructure.Interfaces;

namespace Obilet.Infrastructure.Services;
public class ServiceResponseHelper : IServiceResponseHelper
{
    public ServiceResponse<T> SetError<T>(T? data, string errorMessage = "Post işlemi sırasında bir hata oluştu!", string userMessage = "")
    {
        return new ServiceResponse<T> { Message = errorMessage, UserMessage = userMessage, IsSuccess = false };
    }

    public ServiceResponse<T> SetError<T>(string errorMessage = "Post işlemi sırasında bir hata oluştu!", string userMessage = "")
    {
        return new ServiceResponse<T> { Message = errorMessage, UserMessage = userMessage, IsSuccess = false };
    }

    public ServiceResponse SetError(string errorMessage = "Post işlemi sırasında bir hata oluştu!", string userMessage = "")
    {
        return new ServiceResponse { Message = errorMessage, UserMessage = userMessage, IsSuccess = false };
    }

    public ServiceResponse<T> SetSuccess<T>(T? data)
    {
        return new ServiceResponse<T>{ Data = data, IsSuccess = true };
    }
}
