using System;
using Sandevistan.Core.Hardware;
using Sandevistan.Core.Models;

namespace Sandevistan.Core.Control;

public sealed class StateEstimator
{
    public RobotState Estimate(SensorSnapshot snapshot, RobotState previous)
    {
        if (!SensorSnapshotValidator.IsValid(snapshot))
            throw new ArgumentException("Sensor snapshot contains invalid values.", nameof(snapshot));

        var state = new RobotState
        {
            EmergencyStop = previous.EmergencyStop,
            OperatorEnabled = previous.OperatorEnabled,
            Timestamp = snapshot.TimestampUtc
        };

        foreach (var pair in previous.Joints)
            state.Joints[pair.Key] = pair.Value;

        foreach (var encoder in snapshot.Encoders)
        {
            if (!state.Joints.ContainsKey(encoder.JointId))
                continue;

            var old = state.Joints[encoder.JointId];
            state.Joints[encoder.JointId] = old with
            {
                Position = encoder.PositionRadians,
                Velocity = encoder.VelocityRadiansPerSecond,
                Timestamp = snapshot.TimestampUtc
            };
        }

        return state;
    }
}
