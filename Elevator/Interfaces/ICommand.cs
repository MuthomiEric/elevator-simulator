namespace Elevator.Interfaces
{
    public interface ICommand<TResult> where TResult : ICommandResult { }
}
