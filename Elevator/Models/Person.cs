namespace Elevator.Models
{
    public class Person
    {
        public Person(int at, int to)
        {
            At = at;
            To = to;
        }
        public int At { get; }
        public int To { get; }
    }
}
