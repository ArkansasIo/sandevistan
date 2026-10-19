using Sandevistan.Core.Models;
using Sandevistan.Core.Safety;

namespace Sandevistan.Core.Tests;

public sealed class SafetyTests
{
    [Fact]
    public void WatchdogExpiresAfterTimeout()
    {
        var watchdog = new Watchdog(TimeSpan.FromMilliseconds(10));
        var now = watchdog.LastHeartbeat.AddMilliseconds(11);
        Assert.True(watchdog.IsExpired(now));
    }

    [Fact]
    public void JointLimitValidatorRejectsUnknownJoint()
    {
        var validator = new JointLimitValidator(new[]
        {
            new JointSafetyLimits("knee.left", -1, 1, 2, 20)
        });
        var command = new ActuatorCommand("ankle.left", 0, 0, 0, DateTimeOffset.UtcNow);
        Assert.False(validator.TryValidate(command, out _));
    }

    [Fact]
    public void JointLimitValidatorAcceptsSafeCommand()
    {
        var validator = new JointLimitValidator(new[]
        {
            new JointSafetyLimits("knee.left", -1, 1, 2, 20)
        });
        var command = new ActuatorCommand("knee.left", 0.5, 1, 10, DateTimeOffset.UtcNow);
        Assert.True(validator.TryValidate(command, out _));
    }
}
