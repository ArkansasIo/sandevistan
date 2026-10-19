using Sandevistan.Core.Models;

namespace Sandevistan.Core.Hardware;

public interface IActuatorGateway
{
    Task SendAsync(ActuatorCommand command, CancellationToken cancellationToken);
}
