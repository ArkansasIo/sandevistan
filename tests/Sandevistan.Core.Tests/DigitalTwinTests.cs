using Sandevistan.Core.Models;
using Sandevistan.Core.Simulation;

namespace Sandevistan.Core.Tests;

public sealed class DigitalTwinTests
{
    [Fact]
    public void IntegratesJointMotion()
    {
        var time = new TimeDilationSimulator();
        var twin = new DigitalTwin(time);
        var state = new RobotState { OperatorEnabled = true, Timestamp = DateTimeOffset.UtcNow };
        state.Joints["left-knee"] = new JointState("left-knee", 0, 0, 0, state.Timestamp);

        var next = twin.Step(state, new Dictionary<string, double> { ["left-knee"] = 2.0 }, TimeSpan.FromSeconds(1));

        Assert.Equal(1.0, next.Joints["left-knee"].Position, 6);
        Assert.Equal(2.0, next.Joints["left-knee"].Velocity, 6);
        Assert.Equal(2.0, next.Joints["left-knee"].Acceleration, 6);
    }

    [Fact]
    public void TimeDilationScalesSimulationOnly()
    {
        var time = new TimeDilationSimulator();
        time.Enable(4.0);
        var twin = new DigitalTwin(time);
        var state = new RobotState { Timestamp = DateTimeOffset.UtcNow };
        state.Joints["hip"] = new JointState("hip", 0, 1, 0, state.Timestamp);

        var next = twin.Step(state, new Dictionary<string, double>(), TimeSpan.FromSeconds(0.5));

        Assert.Equal(2.0, next.Joints["hip"].Position, 6);
        Assert.Equal(2.0, (next.Timestamp - state.Timestamp).TotalSeconds, 6);
    }

    [Fact]
    public void EmergencyStopFreezesRunner()
    {
        var time = new TimeDilationSimulator();
        var runner = new SimulationRunner(new DigitalTwin(time), time);
        var state = new RobotState { EmergencyStop = true, Timestamp = DateTimeOffset.UtcNow };
        state.Joints["ankle"] = new JointState("ankle", 0, 1, 0, state.Timestamp);

        var next = runner.Advance(state, new Dictionary<string, double> { ["ankle"] = 100 }, TimeSpan.FromSeconds(1));

        Assert.Same(state, next);
        Assert.Equal(0, next.Joints["ankle"].Position);
    }
}
