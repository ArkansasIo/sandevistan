using Sandevistan.Core.Models;

namespace Sandevistan.Core.Control;

public sealed class FullBodyDigitalTwin
{
    public IReadOnlyList<BodySegment> Segments { get; } = Enum.GetValues<BodySegment>();
    public RobotState State { get; } = new();

    public void UpdateJoint(JointState joint) => State.Joints[joint.JointId] = joint;
}
