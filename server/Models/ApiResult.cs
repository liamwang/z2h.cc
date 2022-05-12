namespace ImageServer.Models;

public class ApiResult
{
    /// <summary>
    /// 代码 0 表示成功，其它表示失败
    /// </summary>
    public int Code { get; set; }

    public string Message { get; set; }

    public static ApiResult Error(string message, int code = 400)
    {
        return new ApiResult { Code = code, Message = message };
    }
}
