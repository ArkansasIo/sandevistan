namespace Sandevistan.Core.Safety;

public sealed record JointSafetyLimits(
    string JointId,
    double MinPositionRadians,
    double MaxPositionRadians,
    double MaxVelocityRadiansPerSecond,
    double MaxTorqueNewtonMeters)
{
    public bool IsValid(out string reason)
    {
        if (string.IsNullOrWhiteSpace(JointId)) { reason = "Joint ID is required."; return false; }
        if (!double.IsFinite(MinPositionRadians) || !double.IsFinite(MaxPositionRadians) ||
            !double.IsFinite(MaxVelocityRadiansPerSecond) || !double.IsFinite(MaxTorqueNewtonMeters))
        { reason = "Safety limits must be finite."; return false; }
        if (MinPositionRadians >= MaxPositionRadians) { reason = "Minimum position must be below maximum position."; return false; }
        if (MaxVelocityRadiansPerSecond <= 0 || MaxTorqueNewtonMeters < 0)
        { reason = "Velocity must be positive and torque cannot be negative."; return false; }
        reason = string.Empty;
        return true;
    }

    public bool Contains(double position, double velocity, double torque) =>
        position >= MinPositionRadians && position <= MaxPositionRadians &&
        Math.Abs(velocity) <= MaxVelocityRadiansPerSecond &&
        Math.Abs(torque) <= MaxTorqueNewtonMeters;
}
