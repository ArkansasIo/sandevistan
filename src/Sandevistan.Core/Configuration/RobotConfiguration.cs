using Sandevistan.Core.Safety;

namespace Sandevistan.Core.Configuration;

public sealed class RobotConfiguration
{
    public string Name { get; init; } = "Sandevistan";
    public bool SimulationOnly { get; init; } = true;
    public TimeSpan WatchdogTimeout { get; init; } = TimeSpan.FromMilliseconds(250);
    public IReadOnlyList<JointSafetyLimits> JointLimits { get; init; } = Array.Empty<JointSafetyLimits>();

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name)) throw new InvalidOperationException("Robot name is required.");
        if (WatchdogTimeout <= TimeSpan.Zero) throw new InvalidOperationException("Watchdog timeout must be positive.");
        if (JointLimits.Count == 0) throw new InvalidOperationException("At least one joint limit is required.");
        foreach (var limit in JointLimits)
            if (!limit.IsValid(out var reason)) throw new InvalidOperationException(reason);
    }
}
