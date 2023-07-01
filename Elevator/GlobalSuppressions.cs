// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Minor Code Smell", "S3267:Loops should be simplified with \"LINQ\" expressions", Justification = "<Pending>", Scope = "member", Target = "~M:Elevator.Manager.ElevatorManager.CallElevator(System.Int32,System.Int32)")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>", Scope = "member", Target = "~M:Elevator.CommandHandler.LiftRequestHandler.Handle(Elevator.Commands.LiftRequestCommand)~Elevator.CommandResult.LiftRequestResult")]
[assembly: SuppressMessage("Major Code Smell", "S1066:Collapsible \"if\" statements should be merged", Justification = "<Pending>", Scope = "member", Target = "~M:Elevator.Manager.ElevatorManager.GetClosestElevator(System.Int32,System.Int32)~Elevator.Models.Elevator")]
