namespace Sandevistan.Core.Models;

public sealed record JointState(
    string JointId,
    double Position,
    double Velocity,
    double Acceleration,
    DateTimeOffset Timestamp);
