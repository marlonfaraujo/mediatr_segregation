namespace MediartSegregation.Application.Ping.UseCases.CreatePing
{
    public class CreatePingResponse
    {
        public string Message { get; set; }
        public CreatePingResponse(string message)
        {
            Message = message;
        }   
    }
}
