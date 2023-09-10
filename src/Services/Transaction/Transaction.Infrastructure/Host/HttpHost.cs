using Transaction.Core.Contracts;

namespace Transaction.Infrastructure.Host;

public class HttpHost : IHttpHost
{
    private HttpClient HttpClient;
    private static readonly string BaseUrl = "http://localhost:5005";
    private static readonly string EndpointAuthentication = "/api/users/{id}";
    public HttpHost()
    {
        HttpClient = new HttpClient();
    }

    public async Task<HttpResponseMessage> SendRequest(HttpMethod method, string uri, HttpContent httpContent, Dictionary<string, string> headers)
    {
        var httpRequestMessage = new HttpRequestMessage(method, uri);
        httpRequestMessage.Content = httpContent;

        httpRequestMessage.Headers.Clear();
        foreach (var header in headers)
        {
            httpRequestMessage.Headers.Add(header.Key, header.Value);
        }

        var httpResponseMessage = new HttpResponseMessage();
        try
        {
            httpResponseMessage = await HttpClient.SendAsync(httpRequestMessage);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return httpResponseMessage;
    }
}