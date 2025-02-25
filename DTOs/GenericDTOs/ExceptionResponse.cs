namespace WebSocketChatApp.DTOs.GenericDTOs
{
    public class ExceptionResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
