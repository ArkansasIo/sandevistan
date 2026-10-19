namespace Sandevistan.Core.Safety;

public sealed class Watchdog
{
    private readonly object _sync = new();
    private DateTimeOffset _lastHeartbeat;

    public Watchdog(TimeSpan timeout)
    {
        if (timeout <= TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(timeout));
        Timeout = timeout;
        _lastHeartbeat = DateTimeOffset.UtcNow;
    }

    public TimeSpan Timeout { get; }

    public DateTimeOffset LastHeartbeat
    {
        get { lock (_sync) return _lastHeartbeat; }
    }

    public void Heartbeat(DateTimeOffset? timestamp = null)
    {
        lock (_sync) _lastHeartbeat = timestamp ?? DateTimeOffset.UtcNow;
    }

    public bool IsExpired(DateTimeOffset? now = null)
    {
        var current = now ?? DateTimeOffset.UtcNow;
        lock (_sync) return current - _lastHeartbeat > Timeout;
    }

    public void Reset() => Heartbeat(DateTimeOffset.UtcNow);
}
