namespace Sandevistan.Core.Telemetry;

public sealed class TelemetryBuffer
{
    private readonly int _capacity;
    private readonly Queue<TelemetryFrame> _frames = new();
    private readonly object _sync = new();

    public TelemetryBuffer(int capacity = 2048)
    {
        if (capacity < 1) throw new ArgumentOutOfRangeException(nameof(capacity));
        _capacity = capacity;
    }

    public int Count { get { lock (_sync) return _frames.Count; } }

    public void Add(TelemetryFrame frame)
    {
        ArgumentNullException.ThrowIfNull(frame);
        lock (_sync)
        {
            _frames.Enqueue(frame);
            while (_frames.Count > _capacity) _frames.Dequeue();
        }
    }

    public IReadOnlyList<TelemetryFrame> Snapshot()
    {
        lock (_sync) return _frames.ToArray();
    }
}
