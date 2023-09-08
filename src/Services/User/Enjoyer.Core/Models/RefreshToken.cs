using System.ComponentModel.DataAnnotations;

namespace Enjoyer.Core.Models
{
    public class ReqRefreshToken
    {
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
