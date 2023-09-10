using Enjoyer.Core.Models;

namespace Enjoyer.Core.DataTransferObjects
{
    public class UserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public string NormalizedEmail { get; set; }
        public string NormalizedUserName { get; set; }
        public string CreatedAt { get; set; }

        public double Balance { get; set; }

        public IEnumerable<Role> Roles { get; set; }

        public UserDto()
        {
            if (string.IsNullOrEmpty(Email))
            {
                Email = UserName;
            }
        }
    }
}
