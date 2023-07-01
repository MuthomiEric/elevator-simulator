using Elevator.Interfaces;

namespace Elevator.CommandResult
{
    public class LiftRequestResult : ICommandResult
    {
        public LiftRequestResult(int currentFloor, string liftName)
        {
            CurrentFloor = currentFloor;
            LiftName = liftName;
        }
        public int CurrentFloor { get; set; }

        public string LiftName { get; set; }
    }
}
