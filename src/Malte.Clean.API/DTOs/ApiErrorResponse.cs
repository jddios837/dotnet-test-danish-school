namespace Malte.Clean.API.DTOs;

public class ApiErrorResponse
{
    public string Title { get; set; } = string.Empty;
    public int Status { get; set; }
    public string Detail { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();
    public string TraceId { get; set; } = string.Empty;

    public static ApiErrorResponse BadRequest(string detail, List<string>? errors = null)
    {
        return new ApiErrorResponse
        {
            Title = "Bad Request",
            Status = 400,
            Detail = detail,
            Errors = errors ?? new List<string>()
        };
    }

    public static ApiErrorResponse NotFound(string detail)
    {
        return new ApiErrorResponse
        {
            Title = "Not Found",
            Status = 404,
            Detail = detail
        };
    }

    public static ApiErrorResponse InternalServerError(string detail)
    {
        return new ApiErrorResponse
        {
            Title = "Internal Server Error",
            Status = 500,
            Detail = detail
        };
    }
}