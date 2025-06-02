using MediartSegregation.Application.Shared.Abstractions;

namespace MediartSegregation.Application.Ping.UseCases.CreatePing
{
    public class CreatePingCommand : IRequestApplication<CreatePingResponse>
    {
        public string Name { get; set; }
        public CreatePingCommand(string name)
        {
            Name = name;
        }

        public CreatePingCommand()
        {
        }
    }
}
