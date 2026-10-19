namespace Sandevistan.Core.Simulation;

public sealed class TimeDilationSimulator
{
    public double SimulationRate { get; private set; } = 1.0;
    public bool Enabled { get; private set; }

    public void Enable(double rate)
    {
        if (!double.IsFinite(rate) || rate < 0.1 || rate > 20.0)
            throw new ArgumentOutOfRangeException(nameof(rate));
        SimulationRate = rate;
        Enabled = true;
    }

    public void Disable()
    {
        SimulationRate = 1.0;
        Enabled = false;
    }

    public TimeSpan Scale(TimeSpan realDelta) =>
        TimeSpan.FromTicks((long)(realDelta.Ticks * SimulationRate));
}
