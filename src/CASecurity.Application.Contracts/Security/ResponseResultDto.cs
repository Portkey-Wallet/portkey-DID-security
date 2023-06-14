using System.Text.Json.Serialization;

namespace CASecurity.Account;

public class ResponseResultDto<T> 
{
    public bool Success { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
    public T Data { get; set; }
}