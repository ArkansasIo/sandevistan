using Sandevistan.Core.Models;

namespace Sandevistan.Core.Simulation;

/// <summary>Deterministic simulation-only joint integrator for the digital twin.</summary>
public sealed class DigitalTwin
{
    private readonly TimeDilationSimulator _time;

    public DigitalTwin(TimeDilationSimulator? time = null) => _time = time ?? new TimeDilationSimulator();

    public RobotState Step(RobotState previous, IReadOnlyDictionary<string, double> accelerations, TimeSpan realDelta)
    {
        if (realDelta < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(realDelta));
        var scaled = _time.Scale(realDelta);
        var dt = scaled.TotalSeconds;
        if (!double.IsFinite(dt) || dt < 0) throw new ArgumentOutOfRangeException(nameof(realDelta));

        var next = new RobotState
        {
            EmergencyStop = previous.EmergencyStop,
            OperatorEnabled = previous.OperatorEnabled,
            Timestamp = previous.Timestamp + scaled
        };

        foreach (var (jointId, joint) in previous.Joints)
        {
            var acceleration = accelerations.TryGetValue(jointId, out var requested) && double.IsFinite(requested) ? requested : 0.0;
            var velocity = joint.Velocity + acceleration * dt;
            var position = joint.Position + joint.Velocity * dt + 0.5 * acceleration * dt * dt;
            next.Joints[jointId] = new JointState(jointId, position, velocity, acceleration, next.Timestamp);
        }
        return next;
    }
}
