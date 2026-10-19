namespace Sandevistan.Core.Models;

public sealed class RobotState
{
    public Dictionary<string, JointState> Joints { get; } = new();
    public bool EmergencyStop { get; set; }
    public bool OperatorEnabled { get; set; }
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
}
