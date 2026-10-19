using Sandevistan.Core.Hardware;
using Sandevistan.Core.Models;
using Sandevistan.Core.Safety;

namespace Sandevistan.Core.Control;

public sealed class ControlLoop
{
    private readonly ISensorGateway _sensors;
    private readonly IActuatorGateway _actuators;
    private readonly StateEstimator _estimator;
    private readonly SafetyController _safety;
    private readonly Watchdog _watchdog;

    public ControlLoop(ISensorGateway sensors, IActuatorGateway actuators, StateEstimator estimator, SafetyController safety, Watchdog watchdog)
    {
        _sensors = sensors;
        _actuators = actuators;
        _estimator = estimator;
        _safety = safety;
        _watchdog = watchdog;
    }

    public async Task<RobotState> UpdateAsync(RobotState previous, IEnumerable<ActuatorCommand> requestedCommands, CancellationToken cancellationToken = default)
    {
        var snapshot = await _sensors.ReadAsync(cancellationToken);
        if (!SensorSnapshotValidator.IsValid(snapshot, out var reason))
        {
            _safety.EmergencyStop();
            throw new InvalidOperationException($"Invalid sensor snapshot: {reason}");
        }

        _watchdog.Heartbeat(snapshot.Timestamp);
        var state = _estimator.Estimate(snapshot, previous);
        foreach (var requested in requestedCommands)
        {
            if (!_safety.TryAuthorize(requested, state, out var safe))
                continue;
            await _actuators.SendAsync(safe, cancellationToken);
        }
        return state;
    }
}
