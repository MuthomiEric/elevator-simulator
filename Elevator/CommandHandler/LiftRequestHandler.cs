using Elevator.CommandResult;
using Elevator.Commands;
using Elevator.Interfaces;
using Elevator.Manager;
#nullable disable
namespace Elevator.CommandHandler
{
    public class LiftRequestHandler : ICommandHandler<LiftRequestCommand, LiftRequestResult>
    {

        public LiftRequestHandler()
        {

        }
        public LiftRequestResult Handle(LiftRequestCommand command, ElevatorManager elevatorController)
        {
            elevatorController.CallElevator(command.FloorAt, command.FloorTo);

            return new LiftRequestResult(1, "");
        }

    }
}
