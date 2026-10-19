# Logic Form

```text
IMU / ENCODERS / FORCE SENSORS
            |
            v
      SENSOR VALIDATION
            |
            v
      STATE ESTIMATOR
            |
            v
       FULL-BODY TWIN
   HEAD/CERVICAL/THORACIC
   LUMBAR/PELVIS/ARMS/LEGS
            |
       +----+----+
       |         |
       v         v
  SIMULATION   MOTION PLANNER
  TIME DILATION       |
       |              v
       +------> SAFETY CONTROLLER <---- E-STOP
                       |
                       v
               ACTUATOR GATEWAY
                 /          \
          SIMULATION      HARDWARE
                           SHADOW/SAFE
```

The fictional Sandevistan effect modifies simulation time only. It never bypasses physical actuator limits.
