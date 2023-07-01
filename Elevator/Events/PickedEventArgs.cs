using Elevator.Models;

namespace Elevator.Events
{
    public class PickedEventArgs : EventArgs
    {
        public int Floor { get; set; }
        public List<Person> People { get; set; }
    }
}
