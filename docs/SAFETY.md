# Safety

This repository is a robotics research scaffold and is not an implant controller.

Required real-hardware protections include an independent emergency stop, power isolation, watchdog, mechanical travel limits, current/torque limits, fault detection, and engineering validation.

`HardwareShadow` is intentionally read-only. `RobotController` is the only application-level actuator path and every command must pass `SafetyController`.

Do not connect this prototype directly to an implanted medical or spinal device.
