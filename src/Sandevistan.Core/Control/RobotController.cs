using Sandevistan.Core.Hardware;
using Sandevistan.Core.Models;
using Sandevistan.Core.Safety;

namespace Sandevistan.Core.Control;

public sealed class RobotController
{
    private readonly SafetyController _safety;
    private readonly IActuatorGateway _gateway;

    public RobotController(SafetyController safety, IActuatorGateway gateway)
    {
        _safety = safety;
        _gateway = gateway;
    }

    public async Task<bool> CommandAsync(ActuatorCommand command, RobotState state, CancellationToken cancellationToken)
    {
        if (!_safety.TryAuthorize(command, state, out var safe)) return false;
        await _gateway.SendAsync(safe, cancellationToken);
        return true;
    }
}
