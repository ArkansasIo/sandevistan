using Sandevistan.Core.Models;

namespace Sandevistan.Core.Safety;

public sealed record SafetyLimits(double MaxPosition, double MaxVelocity, double MaxTorque);

public sealed class SafetyController
{
    private readonly SafetyLimits _limits;
    private bool _estop;

    public SafetyController(SafetyLimits limits) => _limits = limits;

    public void EmergencyStop() => _estop = true;
    public void ResetEmergencyStop() => _estop = false;

    public bool TryAuthorize(ActuatorCommand command, RobotState state, out ActuatorCommand safe)
    {
        safe = command;
        if (_estop || state.EmergencyStop || !state.OperatorEnabled)
        {
            safe = command with { TargetVelocity = 0, TorqueLimit = 0 };
            return false;
        }

        if (!double.IsFinite(command.TargetPosition) ||
            !double.IsFinite(command.TargetVelocity) ||
            !double.IsFinite(command.TorqueLimit)) return false;

        if (Math.Abs(command.TargetPosition) > _limits.MaxPosition ||
            Math.Abs(command.TargetVelocity) > _limits.MaxVelocity ||
            Math.Abs(command.TorqueLimit) > _limits.MaxTorque) return false;

        return true;
    }
}
