

using Obilet.Common;

namespace Obilet.Infrastructure.Interfaces;
public interface IServiceResponseHelper
{
    ServiceResponse<T> SetError<T>(T? data, string errorMessage = "Post işlemi sırasında bir hata oluştu!", string userMessage = "");
    ServiceResponse<T> SetError<T>(string errorMessage = "Post işlemi sırasında bir hata oluştu!", string userMessage = "");
    ServiceResponse SetError(string errorMessage = "Post işlemi sırasında bir hata oluştu!", string userMessage = "");
    ServiceResponse<T> SetSuccess<T>(T? data);
}
