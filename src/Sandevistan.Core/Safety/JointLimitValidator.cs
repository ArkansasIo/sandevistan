using Sandevistan.Core.Models;

namespace Sandevistan.Core.Safety;

public sealed class JointLimitValidator
{
    private readonly IReadOnlyDictionary<string, JointSafetyLimits> _limits;

    public JointLimitValidator(IEnumerable<JointSafetyLimits> limits)
    {
        var list = limits.ToList();
        if (list.Count == 0) throw new ArgumentException("At least one joint limit is required.", nameof(limits));
        foreach (var limit in list)
            if (!limit.IsValid(out var reason)) throw new ArgumentException(reason, nameof(limits));
        _limits = list.ToDictionary(x => x.JointId, StringComparer.Ordinal);
    }

    public bool TryValidate(ActuatorCommand command, out string reason)
    {
        if (!_limits.TryGetValue(command.ActuatorId, out var limit))
        { reason = $"No safety limit is configured for '{command.ActuatorId}'."; return false; }
        if (!double.IsFinite(command.TargetPosition) || !double.IsFinite(command.TargetVelocity) || !double.IsFinite(command.TorqueLimit))
        { reason = "Actuator command contains a non-finite value."; return false; }
        if (!limit.Contains(command.TargetPosition, command.TargetVelocity, command.TorqueLimit))
        { reason = $"Actuator command for '{command.ActuatorId}' exceeds configured limits."; return false; }
        reason = string.Empty;
        return true;
    }
}
