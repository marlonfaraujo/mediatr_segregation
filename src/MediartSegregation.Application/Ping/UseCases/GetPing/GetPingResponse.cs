namespace MediartSegregation.Application.UseCases.Ping.GetPing
{
    public class GetPingResponse
    {
        public GetPingResponse(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}
