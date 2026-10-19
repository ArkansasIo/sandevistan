using Sandevistan.Core.Models;

namespace Sandevistan.Core.Simulation;

public sealed class SimulationRunner
{
    private readonly DigitalTwin _twin;
    private readonly TimeDilationSimulator _time;

    public SimulationRunner(DigitalTwin twin, TimeDilationSimulator time)
    {
        _twin = twin;
        _time = time;
    }

    public RobotState Advance(RobotState state, IReadOnlyDictionary<string, double> accelerations, TimeSpan realDelta)
    {
        if (state.EmergencyStop) return state;
        return _twin.Step(state, accelerations, realDelta);
    }

    public void SetRate(double rate) => _time.Enable(rate);
    public void StopTimeDilation() => _time.Disable();
    public double Rate => _time.SimulationRate;
    public bool Enabled => _time.Enabled;
}
