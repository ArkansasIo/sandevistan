namespace Sandevistan.Core.Models;

public sealed record ActuatorCommand(
    string ActuatorId,
    double TargetPosition,
    double TargetVelocity,
    double TorqueLimit,
    DateTimeOffset Timestamp);
