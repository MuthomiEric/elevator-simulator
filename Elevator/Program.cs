using Elevator.CommandHandler;
using Elevator.Commands;
using Elevator.Manager;
using EventHandler;
using Newtonsoft.Json;
#nullable disable
ElevatorManager elevatorManager = new ElevatorManager(5, 5);

var handler = new LiftRequestHandler();

elevatorManager.ShowElevatorStatus();

var eventHandler = new EventProcessor("localhost:29092", "command");

eventHandler.StartConsuming(message =>
{
    var request = JsonConvert.DeserializeObject<LiftRequestCommand>(message);

    Task.Run(() => handler.Handle(request, elevatorManager));

});