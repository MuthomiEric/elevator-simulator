namespace Elevator.Events
{
    public class FloorReachedEventArgs : EventArgs
    {
        public int Floor { get; set; }
    }
}
