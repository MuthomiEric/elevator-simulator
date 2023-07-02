using Elevator.Enums;
using Elevator.Events;
#nullable disable
namespace Elevator.Models;
public class Elevator
{
    public int CurrentFloor { get; private set; }
    public int MovingTo { get; set; }
    public int FinalDestination { get; set; }
    public bool IsMoving { get; set; }
    public List<Person> People { get; private set; } = new List<Person>();
    public Dictionary<int, List<Person>> PickUps { get; private set; } = new Dictionary<int, List<Person>>();
    public Direction Direction { get; set; }
    public int WeightLimit { get; }
    public string Label { get; }

    #region EVENTS
    public event EventHandler<PickedEventArgs> Picked;

    public event EventHandler<FloorReachedEventArgs> FloorReached;

    public event EventHandler<FinalFloorReachedEventArgs> FinalFloorReached;

    protected virtual void OnFinalFloorReached(int floor)
    {
        FinalFloorReached?.Invoke(this, new FinalFloorReachedEventArgs { Floor = floor });
    }
    protected virtual void EachFloorReached(int floor)
    {
        FloorReached?.Invoke(this, new FloorReachedEventArgs { Floor = floor });
    }
    protected virtual void Passegers_PickedAtFloor(int floor, List<Person> people)
    {
        Picked?.Invoke(this, new PickedEventArgs { Floor = floor, People = people });
    }

    #endregion
    public Elevator(int weightLimit, string label)
    {
        CurrentFloor = 1;
        IsMoving = false;
        Direction = Direction.Stopped;
        WeightLimit = weightLimit;
        Label = label;
    }

    public void SendTo(int callingAt, int goingTo, Direction direction)
    {

        if (IsMoving)
        {
            if (Direction == Direction.Up)
            {
                // Passeger could be going in the opposite direction
                if (!direction.Equals(Direction))
                    FinalDestination = goingTo < FinalDestination ? goingTo : FinalDestination;

                return;
            }
            else
            {
                // Could be going in the opposite direction
                if (!direction.Equals(Direction))
                    FinalDestination = goingTo > FinalDestination ? goingTo : FinalDestination;

                return;
            }
        }

        // Called on a resting elevator only
        Move(direction, callingAt, goingTo);
    }

    public void Move(Direction direction, int from, int to)
    {
        IsMoving = true;

        Direction = direction;

        MovingTo = from;

        FinalDestination = to;

        while (MoveWhen(Direction, from, to))
        {
            // Drop first to create space the pick
            Drop();

            PickUp();

            Thread.Sleep(TimeSpan.FromSeconds(2));

            EachFloorReached(CurrentFloor);

            if (MovingTo == CurrentFloor)
            {
                OnFinalFloorReached(CurrentFloor);

                break;
            }

            if (Direction == Direction.Up)
                CurrentFloor++;

            if (Direction == Direction.Down)
                CurrentFloor--;
        }
    }

    // This helps to check when the lift should be moving
    public bool MoveWhen(Direction direction, int from, int to)
    {
        if (direction == Direction.Up)
        {
            return CurrentFloor <= from;
        }

        if (direction == Direction.Down)
        {
            return CurrentFloor >= to;
        }

        return false;
    }

    private void PickUp()
    {
        if (PickUps.ContainsKey(CurrentFloor))
        {
            // Get passeger going to the same directions
            List<Person> pickUps = new List<Person>();

            if (Direction == Direction.Up)
            {
                pickUps = PickUps[CurrentFloor].Where(p => p.To > CurrentFloor).ToList();
            }
            if (Direction == Direction.Down)
            {
                pickUps = PickUps[CurrentFloor].Where(p => p.To < CurrentFloor).ToList();
            }
            var counter = 0;

            foreach (var person in pickUps)
            {
                if (!AddAPerson(person))
                    break;

                PickUps[CurrentFloor].Remove(person);

                counter++;
            }

            // Remove the pickups if all passegers picked
            if (!PickUps[CurrentFloor].Any())
                PickUps.Remove(CurrentFloor);

            if (counter != 0)
            {
                Console.WriteLine($"{Label} Picked {counter} at floor {CurrentFloor}");

                // This event helps to remove picked passegers from a floor
                Passegers_PickedAtFloor(CurrentFloor, pickUps);
            }
        }

    }

    private void Drop()
    {
        var peopleToDrop = People.Where(p => p.To == CurrentFloor).ToList();

        if (peopleToDrop.Any())
        {
            var count = peopleToDrop.Count;

            foreach (var person in peopleToDrop)
            {
                RemoveAPerson(person);
            }

            Console.WriteLine($"{Label} Dropped {count} at floor {CurrentFloor}");

            Console.WriteLine($"{Label}:");
            Console.WriteLine($"- Current floor: {CurrentFloor}");
            Console.WriteLine($"- Direction: {Direction.Stopped}");
            Console.WriteLine($"- Number of people: {People.Count}");
            Console.WriteLine("*******************************************************");

        }
    }

    public int GetDistance(int from, int to)
    {
        var dir = from > to ? Direction.Down : Direction.Up;
        // Means moving on the same direction
        if (Direction.Equals(dir))
        {
            if (dir == Direction.Up && CurrentFloor < from)
                return CurrentFloor - to;
            if (dir == Direction.Down && CurrentFloor > from)
                return CurrentFloor - to;
        }

        if (Direction.Equals(Direction.Stopped) && CurrentFloor >= from)
            return CurrentFloor - to;

        return ((Math.Abs(CurrentFloor - from)) + Math.Abs(from - to));
    }

    public bool AddAPerson(Person person)
    {
        if (People.Count >= WeightLimit)
            return false;

        People.Add(person);

        return true;
    }

    private void RemoveAPerson(Person person)
    {
        People.Remove(person);
    }
}
