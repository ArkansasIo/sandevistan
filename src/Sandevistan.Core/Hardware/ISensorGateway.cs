using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace Sandevistan.Core.Hardware;

/// <summary>
/// Read-only sensor abstraction for the external Sandevistan robotics platform.
/// This layer only acquires and validates observations; it never commands actuators.
/// </summary>
public interface ISensorGateway
{
    Task<SensorSnapshot> ReadAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Immutable collection of synchronized sensor observations.
/// </summary>
public sealed record SensorSnapshot(
    DateTimeOffset Timestamp,
    IReadOnlyList<ImuSample> Imus,
    IReadOnlyList<EncoderSample> Encoders,
    IReadOnlyList<ForceTorqueSample> ForceTorques)
{
    public static SensorSnapshot Empty(DateTimeOffset? timestamp = null) =>
        new(
            timestamp ?? DateTimeOffset.UtcNow,
            Array.Empty<ImuSample>(),
            Array.Empty<EncoderSample>(),
            Array.Empty<ForceTorqueSample>());
}

/// <summary>
/// Six-axis inertial measurement unit observation.
/// Acceleration is expressed in m/s^2 and angular velocity in rad/s.
/// </summary>
public sealed record ImuSample(
    string SensorId,
    DateTimeOffset Timestamp,
    Vector3 LinearAcceleration,
    Vector3 AngularVelocity,
    Vector3 MagneticField,
    double TemperatureCelsius);

/// <summary>
/// Joint/actuator position observation.
/// Position is expressed in radians and velocity in rad/s.
/// </summary>
public sealed record EncoderSample(
    string SensorId,
    string JointId,
    DateTimeOffset Timestamp,
    double PositionRadians,
    double VelocityRadiansPerSecond,
    double? CurrentAmps,
    double? TemperatureCelsius);

/// <summary>
/// Six-axis force/torque sensor observation.
/// Force is expressed in newtons and torque in newton-metres.
/// </summary>
public sealed record ForceTorqueSample(
    string SensorId,
    DateTimeOffset Timestamp,
    Vector3 ForceNewtons,
    Vector3 TorqueNewtonMeters);

/// <summary>
/// Basic sanity checks used before sensor data enters the state estimator.
/// </summary>
public static class SensorSnapshotValidator
{
    public static bool IsValid(SensorSnapshot snapshot, out string reason)
    {
        if (snapshot is null)
        {
            reason = "Sensor snapshot is null.";
            return false;
        }

        if (snapshot.Timestamp == default)
        {
            reason = "Sensor snapshot timestamp is invalid.";
            return false;
        }

        foreach (var imu in snapshot.Imus)
        {
            if (string.IsNullOrWhiteSpace(imu.SensorId))
            {
                reason = "IMU sensor ID is missing.";
                return false;
            }

            if (!IsFinite(imu.LinearAcceleration) ||
                !IsFinite(imu.AngularVelocity) ||
                !IsFinite(imu.MagneticField) ||
                !double.IsFinite(imu.TemperatureCelsius))
            {
                reason = $"IMU '{imu.SensorId}' contains a non-finite value.";
                return false;
            }
        }

        foreach (var encoder in snapshot.Encoders)
        {
            if (string.IsNullOrWhiteSpace(encoder.SensorId) ||
                string.IsNullOrWhiteSpace(encoder.JointId))
            {
                reason = "Encoder sensor or joint ID is missing.";
                return false;
            }

            if (!double.IsFinite(encoder.PositionRadians) ||
                !double.IsFinite(encoder.VelocityRadiansPerSecond) ||
                (encoder.CurrentAmps.HasValue && !double.IsFinite(encoder.CurrentAmps.Value)) ||
                (encoder.TemperatureCelsius.HasValue && !double.IsFinite(encoder.TemperatureCelsius.Value)))
            {
                reason = $"Encoder '{encoder.SensorId}' contains a non-finite value.";
                return false;
            }
        }

        foreach (var forceTorque in snapshot.ForceTorques)
        {
            if (string.IsNullOrWhiteSpace(forceTorque.SensorId))
            {
                reason = "Force/torque sensor ID is missing.";
                return false;
            }

            if (!IsFinite(forceTorque.ForceNewtons) || !IsFinite(forceTorque.TorqueNewtonMeters))
            {
                reason = $"Force/torque sensor '{forceTorque.SensorId}' contains a non-finite value.";
                return false;
            }
        }

        reason = string.Empty;
        return true;
    }

    private static bool IsFinite(Vector3 value) =>
        float.IsFinite(value.X) &&
        float.IsFinite(value.Y) &&
        float.IsFinite(value.Z);
}
