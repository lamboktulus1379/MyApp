using Microsoft.Extensions.Configuration;
using Transaction.Core.Contracts;
using Transaction.Infrastructure.Host.UserClient.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text.Json;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;


namespace Transaction.Infrastructure.UserClient;

public class UserClient : IUserClient
{
    public IConfiguration Configuration;
    private readonly IHttpHost _host;

    public UserClient(IConfiguration configuration, IHttpHost host)
    {
        Configuration = configuration;
        _host = host;
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
        var userResponse = JsonConvert.DeserializeObject<UserCTO>(content);

        return userResponse;
    }
}