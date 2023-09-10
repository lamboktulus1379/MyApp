namespace Transaction.Core.Contracts;
public interface IHttpHost
{
    public Task<HttpResponseMessage> SendRequest(HttpMethod method, string uri, HttpContent httpContent, Dictionary<string, string> headers);
}