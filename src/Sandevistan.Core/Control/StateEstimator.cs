using System;
using Sandevistan.Core.Hardware;

namespace Sandevistan.Core.Control;

public sealed class StateEstimator
{
    public RobotState Estimate(SensorSnapshot snapshot, RobotState previous)
    {
        if (!SensorSnapshotValidator.IsValid(snapshot))
            throw new ArgumentException("Sensor snapshot contains invalid values.", nameof(snapshot));

        var state = previous.Clone();
        state.TimestampUtc = snapshot.TimestampUtc;

        foreach (var encoder in snapshot.Encoders)
        {
            var joint = state.Joints.Find(j => j.JointId == encoder.JointId);
            if (joint is null) continue;
            joint.PositionRadians = encoder.PositionRadians;
            joint.VelocityRadiansPerSecond = encoder.VelocityRadiansPerSecond;
        }

        state.LastImu = snapshot.Imu;
        state.LastForceTorque = snapshot.ForceTorque;
        return state;
    }
}
