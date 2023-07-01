using Elevator.Commands;
using EventHandler;

var eventHandler = new EventProcessor("localhost:29092", "command");

int numberOfFloors = 20, startingFrom = 1;

while (true)
{
    (var fromFloor, var toFloor) = (0, 0);

    while (fromFloor.Equals(toFloor))
    {
        Console.WriteLine($"Enter current floor:");

        (fromFloor, toFloor) = GetInput(numberOfFloors, startingFrom);
    }

    var liftRequestcommand = new LiftRequestCommand(fromFloor, toFloor);

    eventHandler.PublishMessage(liftRequestcommand);

}
static (int fromFloor, int toFloor) GetInput(int numberOfFloors, int startingFrom)
{
    int fromFloor;

    int toFloor;

    while (!int.TryParse(Console.ReadLine(), out fromFloor) || fromFloor < startingFrom || fromFloor > numberOfFloors)
    {
        Console.WriteLine($"Invalid !! please enter current floor again:");
    }

    Console.WriteLine($"Enter the floor to go to:");

    while (!int.TryParse(Console.ReadLine(), out toFloor) || toFloor < startingFrom || toFloor > numberOfFloors)
    {
        Console.WriteLine($"Invalid !! please enter the floor to go to again:");
    }

    return (fromFloor, toFloor);
}
