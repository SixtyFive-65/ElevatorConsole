using ElevatorMovement.Services.Implementation;
using ElevatorMovement.Services.Interface;

class Program
{
    static void Main()
    {
        var elevatorId = 1;
        var totalFloors = 10;

        try
        {
            IBuilding building = new Building(totalFloors);

            Console.WriteLine("Starting Elevator Simulation...");

            var elevator = new PassengerElevator(elevatorId, totalFloors);

            building.Elevators.Add(elevator);

            while (true)
            {
                var currentFloor = ReadCurrentFloor(totalFloors, "Current");

                var destinationFloor = ReadCurrentFloor(totalFloors, "Destination");

                Passenger passenger = new Passenger(currentFloor, destinationFloor);
                passenger.RequestElevator(building);

                foreach (var e in building.Elevators)
                {
                    e.Move();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex?.InnerException?.ToString() ?? ex?.Message);
        }
    }

    static int ReadCurrentFloor(int totalFloors, string floorDescription)
    {
        int floor;

        do
        {
            Console.WriteLine($"Enter the {floorDescription} floor (1 to {totalFloors}):");
            if (int.TryParse(Console.ReadLine(), out floor) && floor > 0 && floor <= totalFloors)
            {
                return floor;
            }

            Console.WriteLine($"Invalid floor. Please select a floor between 1 and {totalFloors}.");
        }
        while (true);
    }
}