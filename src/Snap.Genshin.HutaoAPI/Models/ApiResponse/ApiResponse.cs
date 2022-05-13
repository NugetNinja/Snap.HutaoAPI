using System.Text.Json.Serialization;

namespace Snap.HutaoAPI.Models
{
    public class ApiResponse<T>
    {
        public ApiResponse(ApiCode code, string message, T data)
        {
            Code = code;
            Message = message;
            Data = data;
        }

        [JsonPropertyName("retcode")]
        public ApiCode Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }
}
