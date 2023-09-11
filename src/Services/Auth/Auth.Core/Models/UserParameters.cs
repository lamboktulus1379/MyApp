namespace Auth.Core.Models
{
    public class UserParameters : QueryStringParameters
    {
        public UserParameters()
        {
            OrderBy = "DateCreated";
        }
        public string Email { get; set; }
        public string WhereIn { get; set; }
    }
}
