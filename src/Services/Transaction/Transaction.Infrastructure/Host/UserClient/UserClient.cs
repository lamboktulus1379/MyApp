using Microsoft.Extensions.Configuration;
using Transaction.Core.Contracts;
using Transaction.Infrastructure.Host.UserClient.Models;
using Newtonsoft.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;


namespace Transaction.Infrastructure.UserClient;

public class UserClient : IUserClient
{
    public IConfiguration Configuration;
    private readonly IHttpHost _host;
    private readonly ILogger<UserClient> _logger;

    public UserClient(IConfiguration configuration, IHttpHost host, ILogger<UserClient> logger)
    {
        Configuration = configuration;
        _host = host;
        _logger = logger;
    }
    public async Task<UserCTO> GetUser(string Id)
    {
        var settingsContent = new JsonSerializerOptions()
        {
            WriteIndented = true,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

        };
        HttpContent httpContent = null;
        string uri = $"http://localhost:5005/api/users/{Id}";

        Dictionary<string, string> headers = new Dictionary<string, string>()
        {
        };

        HttpResponseMessage response = await _host.SendRequest(HttpMethod.Get, uri, httpContent, headers);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {

            }
        }

        _logger.LogInformation($"User {content}");

        var userResponse = JsonConvert.DeserializeObject<UserCTO>(content);

        return userResponse;
    }
}