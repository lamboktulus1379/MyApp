using Newtonsoft.Json;

namespace Enjoyer.Core.Models
{
    public class ValidateUser
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
