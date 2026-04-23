// This file defines a generic class `ApiResponse<T>` that represents a standard structure for API responses.
// It includes properties to indicate success, a message, the data being returned, and any errors that may have occurred.
// The class also provides static methods for creating successful and failed responses. Additionally,
// there is a non-generic version of `ApiResponse` for cases where no data is needed.

namespace Application.Common;

public class ApiResponse<T> 
{
    
    public bool Success { get; set; }
   
    public string Message { get; set; } = string.Empty;
    
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }

    public static ApiResponse<T> Ok(T data, string message = "Success") =>
        new() { Success = true, Message = message, Data = data };

    public static ApiResponse<T> Fail(string message, List<string>? errors = null) =>
        new() { Success = false, Message = message, Errors = errors };
}

public class ApiResponse : ApiResponse<object> 
{
    public static ApiResponse Ok(string message = "Success") =>
        new() { Success = true, Message = message };

    public static new ApiResponse Fail(string message, List<string>? errors = null) =>
        new() { Success = false, Message = message, Errors = errors };
}

