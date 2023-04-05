namespace CheapConnect.NET;

public class CheapConnectException : Exception
{
    public int? ErrorCode { get; set; }

    public CheapConnectException(string message, int? errorCode = null): base(message) => ErrorCode = errorCode;
}