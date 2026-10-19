using Sandevistan.Core.Models;

namespace Sandevistan.Core.Telemetry;

public sealed record TelemetryFrame(
    DateTimeOffset Timestamp,
    RobotState State,
    IReadOnlyList<ActuatorCommand> Commands,
    bool SimulationOnly,
    string Mode);
