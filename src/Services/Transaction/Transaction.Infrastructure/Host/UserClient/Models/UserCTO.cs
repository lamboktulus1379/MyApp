namespace Transaction.Infrastructure.Host.UserClient.Models;

public class ResCTO
{
    public string ResponseMessage { get; set; }
    public string ResponseCode { get; set; }
    public UserCTO Data { get; set; }
}

public class UserCTO
{
    public double Balance { get; set; }
}