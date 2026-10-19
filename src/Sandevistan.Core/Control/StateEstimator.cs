using Sandevistan.Core.Hardware;
using Sandevistan.Core.Models;

namespace Sandevistan.Core.Control;

public sealed class StateEstimator
{
    public RobotState Estimate(SensorSnapshot snapshot, RobotState previous)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentNullException.ThrowIfNull(previous);

        if (!SensorSnapshotValidator.IsValid(snapshot, out var reason))
            throw new ArgumentException(reason, nameof(snapshot));

        var state = new RobotState
        {
            EmergencyStop = previous.EmergencyStop,
            OperatorEnabled = previous.OperatorEnabled,
            Timestamp = snapshot.Timestamp
        };

        foreach (var pair in previous.Joints)
            state.Joints[pair.Key] = pair.Value;

        foreach (var encoder in snapshot.Encoders)
        {
            if (!state.Joints.TryGetValue(encoder.JointId, out var old))
                continue;

            state.Joints[encoder.JointId] = old with
            {
                Position = encoder.PositionRadians,
                Velocity = encoder.VelocityRadiansPerSecond,
                Timestamp = encoder.Timestamp
            };
        }

        return state;
    }
}
