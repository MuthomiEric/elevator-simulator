using Elevator.Enums;
using Elevator.Events;
using Elevator.Models;
#nullable disable
namespace Elevator.Manager;
public class ElevatorManager
{
    static readonly object obj = new object();

    private readonly List<Models.Elevator> _elevators;

    private readonly Dictionary<int, List<Person>> _floors;

    private readonly List<Models.Elevator> _availableElevators = new List<Models.Elevator>();

    public ElevatorManager(int numberOfElevators, int weightLimit)
    {
        _elevators = new List<Models.Elevator>();

        _floors = new Dictionary<int, List<Person>>();

        var labels = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        for (int i = 0; i < numberOfElevators; i++)
        {
            var lift = new Models.Elevator(weightLimit, $"Lift {labels[i]}");

            lift.FloorReached += Lift_FloorReached;

            lift.FinalFloorReached += FinalFloorReached;

            lift.Picked += Passegers_Picked;

            _availableElevators.Add(lift);

            _elevators.Add(lift);
        }
    }

    #region EVENTS
    private void Passegers_Picked(object sender, PickedEventArgs e)
    {
        if (_floors.ContainsKey(e.Floor))
        {
            // Remove picked passegers
            foreach (var person in e.People)
            {
                _floors[e.Floor].Remove(person);
            }
            // If all picked remove the floor
            if (!_floors[e.Floor].Any())
                _floors.Remove(e.Floor);
        }
    }

    // Each elevator emits this event on each floor, which helps the manager to monitor the lifts
    // also helps to know which lift is free or can be assigned a task
    private void Lift_FloorReached(object sender, FloorReachedEventArgs e)
    {
        var lift = (Models.Elevator)sender;

        if (lift.People.Count < lift.WeightLimit)
        {
            if (!_availableElevators.Contains(lift))
                _availableElevators.Add(lift);
        }
        else
        {
            _availableElevators.Remove(lift);
        }

        if (lift.MovingTo == lift.CurrentFloor)
        {
            lift.IsMoving = false;

            lift.Direction = Direction.Stopped;
        }
        else
        {
            lock (obj)
            {
                Console.WriteLine($"{lift.Label}:");
                Console.WriteLine($"- Current floor: {lift.CurrentFloor}");
                Console.WriteLine($"- Direction: {lift.Direction}");
                Console.WriteLine($"- Number of people: {lift.People.Count}");
                Console.WriteLine("*******************************************************");
            }
        }
    }

    // Fired by each elevator whenever it reaches the picking or dropping floor
    private void FinalFloorReached(object sender, FinalFloorReachedEventArgs e)
    {
        var lift = (Models.Elevator)sender;

        var counter = 0;

        if (_floors.ContainsKey(lift.CurrentFloor))
        {
            foreach (var person in _floors[lift.CurrentFloor].ToList())
            {
                if (!lift.AddAPerson(person))
                    break;

                // Remove each picked passager
                _floors[lift.CurrentFloor].Remove(person);

                counter++;
            }
            // If all passager were picked remove the floor
            if (!_floors[lift.CurrentFloor].Any())
                _floors.Remove(lift.CurrentFloor);
        }

        if (lift.FinalDestination != lift.CurrentFloor)
        {
            if (counter != 0)
            {
                Console.WriteLine($"{lift.Label} Picked {counter} at floor {lift.CurrentFloor}");

                Console.WriteLine($"{lift.Label}:");
                Console.WriteLine($"- Current floor: {lift.CurrentFloor}");
                Console.WriteLine($"- Direction: {lift.Direction}");
                Console.WriteLine($"- Number of people: {lift.People.Count}");
                Console.WriteLine("*******************************************************");
            }

            var direction = GetDirection(lift);

            lift.SendTo(lift.FinalDestination, lift.FinalDestination, direction);
        }
    }

    #endregion
    private Direction GetDirection(Models.Elevator lift)
    {
        if (lift.FinalDestination > lift.CurrentFloor)
            return Direction.Up;

        else
            return Direction.Down;
    }

    public void CallElevator(int fromFloor, int toFloor)
    {
        if (_floors.ContainsKey(fromFloor))
        {
            _floors[fromFloor].Add(new Person(fromFloor, toFloor));
        }
        else
        {
            _floors.Add(fromFloor, new List<Person>());

            _floors[fromFloor].Add(new Person(fromFloor, toFloor));
        }

        if (toFloor.Equals(fromFloor))
        {
            Console.WriteLine("Please select the floor!!");

            return;
        }

        var direction = toFloor > fromFloor ? Direction.Up : Direction.Down;

        Models.Elevator closestElevator = GetClosestElevator(fromFloor, toFloor, direction);

        var callerDirection = closestElevator.FinalDestination > closestElevator.CurrentFloor ? Direction.Down : Direction.Up;

        closestElevator.SendTo(fromFloor, toFloor, callerDirection);
    }

    private Models.Elevator GetClosestElevator(int floor, int floorTo, Direction direction)
    {
        Models.Elevator closestElevator = null;

        int minDistance = int.MaxValue;

        foreach (Models.Elevator elevator in _availableElevators)
        {
            int distance = Math.Abs(elevator.CurrentFloor - floor);

            if (elevator.IsMoving)
            {
                if (elevator.CanPick(direction, floor))
                {
                    if (elevator.PickUps.ContainsKey(floor))
                    {
                        elevator.PickUps[floor].Add(new Person(floor, floorTo));
                    }
                    else
                    {
                        elevator.PickUps.Add(floor, new List<Person>());

                        elevator.PickUps[floor].Add(new Person(floor, floorTo));
                    }

                    return elevator;
                }

                distance = int.MaxValue;
            }

            if (distance < minDistance)
            {
                minDistance = distance;

                closestElevator = elevator;
            }
        }

        return closestElevator;
    }

    public void ShowElevatorStatus()
    {
        for (int i = 0; i < _elevators.Count; i++)
        {
            Models.Elevator elevator = _elevators[i];

            Console.WriteLine($"{elevator.Label}:");
            Console.WriteLine($"- Current floor: {elevator.CurrentFloor}");
            Console.WriteLine($"- Direction: {elevator.Direction}");
            Console.WriteLine($"- Number of people: {elevator.People.Count}");
            Console.WriteLine("-----------------------------------------------------------");
        }
    }

}
