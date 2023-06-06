namespace Obilet.Common;
public class ServiceResponse<TResult> : ServiceResponse
{
    public TResult? Data { get; set; }
}
