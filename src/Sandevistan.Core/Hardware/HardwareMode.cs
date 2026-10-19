namespace Sandevistan.Core.Hardware;

public enum HardwareMode
{
    Simulation,
    ShadowReadOnly,
    ExternalActuator
}

public sealed record HardwarePolicy(HardwareMode Mode, bool PhysicalOutputEnabled)
{
    public static HardwarePolicy Simulation => new(HardwareMode.Simulation, false);
    public static HardwarePolicy Shadow => new(HardwareMode.ShadowReadOnly, false);

    public void Validate()
    {
        if (Mode != HardwareMode.ExternalActuator && PhysicalOutputEnabled)
            throw new InvalidOperationException("Physical actuator output is only valid in ExternalActuator mode.");
    }
}
