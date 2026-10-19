# Sandevistan Robotics

Sandevistan-inspired full-body external robotics research platform.

This project models a cybernetic/exoskeleton control stack in C# with a full-body digital twin, sensor and actuator abstraction, simulation-only time dilation, telemetry, and a mandatory safety controller.

## Safety boundary

This is a robotics research scaffold, not an implant controller. The Sandevistan effect is software simulation only. Real hardware must have independent emergency-stop, power isolation, current/torque limits, watchdogs, mechanical limits, and qualified engineering review.

## Modes

- `Simulation` — digital twin and simulated actuators.
- `HardwareShadow` — sensor/read-only integration; no actuator writes.
- `HardwareSafe` — bounded commands through the safety layer.

## Structure

```text
src/Sandevistan.Core/
  Models/
  Safety/
  Hardware/
  Control/
  Simulation/
src/Sandevistan.App/
tests/Sandevistan.Core.Tests/
docs/
sql/
config/
```

Target framework: .NET 8.
