namespace Transaction.Core.DataTransferObjects;

public class Res
{
    public string ResponseMessage { get; set; }
    public string ResponseCode { get; set; }
    public object Data { get; set; }
}