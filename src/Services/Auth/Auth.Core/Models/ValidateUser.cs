using Newtonsoft.Json;

namespace Auth.Core.Models
{
    public class ValidateUser
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
