using Sandevistan.Core.Models;
using Sandevistan.Core.Safety;

namespace Sandevistan.Core.Control;

public sealed class MotionPlanner
{
    private readonly JointLimitValidator _validator;

    public MotionPlanner(JointLimitValidator validator) => _validator = validator;

    public bool TryPlan(string jointId, double targetPosition, double maxVelocity, double torqueLimit, DateTimeOffset timestamp, out ActuatorCommand command, out string reason)
    {
        command = new ActuatorCommand(jointId, targetPosition, maxVelocity, torqueLimit, timestamp);
        if (!_validator.TryValidate(command, out reason))
        {
            command = command with { TargetVelocity = 0, TorqueLimit = 0 };
            return false;
        }
        return true;
    }
}
