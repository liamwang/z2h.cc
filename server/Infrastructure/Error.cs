namespace Server;

public enum ErrorCode
{
    Unhandled = 0,
    BadRequest = 400,
    ServerError = 500
}

public class Error : Exception
{
    public Error(string message, ErrorCode code) : base(message)
    {
        Code = code;
    }

    public ErrorCode Code { get; }

    public static void Throw(string message, ErrorCode code)
    {
        throw new Error(message, code);
    }

    public static void ThrowIf(bool predicate, string message, ErrorCode code)
    {
        if (predicate) Throw(message, code);
    }

    public static void ThrowBadRequest(string message)
    {
        throw new Error(message, ErrorCode.BadRequest);
    }

    public static void ThrowBadRequestIf(bool predicate, string message)
    {
        if (predicate) Throw(message, ErrorCode.BadRequest);
    }
}
