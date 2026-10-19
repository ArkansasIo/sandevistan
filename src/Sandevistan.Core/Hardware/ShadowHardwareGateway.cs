using Sandevistan.Core.Models;

namespace Sandevistan.Core.Hardware;

public sealed class ShadowHardwareGateway : IActuatorGateway
{
    public Task SendAsync(ActuatorCommand command, CancellationToken cancellationToken) =>
        throw new InvalidOperationException("HardwareShadow mode cannot command actuators.");
}
