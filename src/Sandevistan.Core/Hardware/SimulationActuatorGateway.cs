using Sandevistan.Core.Models;

namespace Sandevistan.Core.Hardware;

public sealed class SimulationActuatorGateway : IActuatorGateway
{
    public List<ActuatorCommand> History { get; } = new();

    public Task SendAsync(ActuatorCommand command, CancellationToken cancellationToken)
    {
        History.Add(command);
        return Task.CompletedTask;
    }
}
