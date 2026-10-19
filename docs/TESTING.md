# Testing

## Local commands

```bash
dotnet restore src/Sandevistan.App/Sandevistan.App.csproj
dotnet build src/Sandevistan.App/Sandevistan.App.csproj --configuration Release
dotnet test tests/Sandevistan.Core.Tests/Sandevistan.Core.Tests.csproj --configuration Release
```

## Required safety tests

1. Watchdog expiry and reset.
2. Non-finite sensor values are rejected.
3. Unknown joints are rejected.
4. Position, velocity, and torque limits are enforced.
5. Emergency stop prevents actuator authorization.
6. Operator-disabled state prevents actuator authorization.
7. Simulation mode never requires physical actuator output.
