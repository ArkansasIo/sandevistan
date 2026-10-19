# Sandevistan Architecture

## Runtime pipeline

```text
Sensors
  -> SensorSnapshotValidator
  -> StateEstimator
  -> RobotState / Digital Twin
  -> MotionPlanner
  -> SafetyController + JointLimitValidator
  -> IActuatorGateway
  -> Simulation / Shadow / External hardware boundary
```

## Core modules

- `Hardware`: sensor and actuator abstractions plus hardware mode policy.
- `Models`: immutable command and observation records and robot state.
- `Control`: state estimation, digital twin, motion planning, and control loop.
- `Safety`: global limits, joint limits, emergency stop, and watchdog.
- `Telemetry`: bounded in-memory telemetry storage.
- `Simulation`: time-dilation visualization/simulation only.
- `Configuration`: validated runtime configuration.

## Operating modes

### Simulation
No physical actuator output. Intended for development and automated tests.

### ShadowReadOnly
Sensors may be observed, but the system does not authorize physical output.

### ExternalActuator
A separate, independently reviewed hardware safety layer must enforce physical limits, emergency stop, power isolation, and watchdog behavior. The software command pipeline does not bypass that layer.

## Time dilation

The Sandevistan time-dilation feature is a simulation parameter. It does not increase human physiological speed or override real-world actuator safety limits.
