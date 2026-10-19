using Sandevistan.Core.Control;
using Sandevistan.Core.Hardware;
using Sandevistan.Core.Models;
using Sandevistan.Core.Safety;
using Sandevistan.Core.Simulation;

var safety = new SafetyController(new SafetyLimits(1.0, 2.0, 10.0));
var gateway = new SimulationActuatorGateway();
var controller = new RobotController(safety, gateway);
var twin = new FullBodyDigitalTwin();
var sandy = new TimeDilationSimulator();

twin.State.OperatorEnabled = true;
sandy.Enable(4.0);

var command = new ActuatorCommand("left-knee", 0.2, 0.5, 2.0, DateTimeOffset.UtcNow);
var accepted = await controller.CommandAsync(command, twin.State, CancellationToken.None);

Console.WriteLine($"Simulation rate: {sandy.SimulationRate}x");
Console.WriteLine($"Command accepted: {accepted}");
Console.WriteLine($"Recorded commands: {gateway.History.Count}");
