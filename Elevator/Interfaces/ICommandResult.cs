namespace Elevator.Interfaces
{
    public interface ICommandResult
    {
        int CurrentFloor { get; set; }
        string LiftName { get; set; }
    }
}
