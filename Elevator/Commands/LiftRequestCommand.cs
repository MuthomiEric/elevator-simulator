using Elevator.CommandResult;

namespace Elevator.Commands
{
    public class LiftRequestCommand : Interfaces.ICommand<LiftRequestResult>
    {
        public LiftRequestCommand(int floorAt, int floorTo)
        {
            FloorAt = floorAt;
            FloorTo = floorTo;
        }

        public int FloorAt { get; set; }
        public int FloorTo { get; set; }
    }
}
