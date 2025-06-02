using MediartSegregation.Application.Shared.Abstractions;

namespace MediartSegregation.Application.UseCases.Ping.GetPing
{
    public class GetPingQuery : IRequestApplication<GetPingResponse>
    {
        public string Name { get; set; }
        public GetPingQuery(string name)
        {
            Name = name;
        }

        public GetPingQuery()
        {
        }
    }
}
