using ElevatorMovement.Services.Base;
using ElevatorMovement.Services.Implementation;
using ElevatorMovement.Services.Interface;

class Program
{
    static void Main()
    {
        var elevatorId = 1;   //we can allow the capture of elevators and floors
        var totalFloors = 10;

        try
        {
            IBuilding building = new Building(totalFloors);

            Console.WriteLine("Starting Elevator Simulation...");

            var elevator = new PassengerElevator(elevatorId, totalFloors);  // From Elevator Base, we can create different elevator types

            building.Elevators.Add(elevator);

            while (true)
            {
                var currentFloor = ReadCurrentFloor(totalFloors, "Current");
                var destinationFloor = ReadCurrentFloor(totalFloors, "Destination");

                // Request the elevator
                elevator.RequestElevator(currentFloor, destinationFloor);

                // Move the elevator and process requests
                foreach (var e in building.Elevators)
                {
                    e.Move();
                    Console.WriteLine($"Elevator {e.Id} currently has {((Elevator)e).GetPassengerCount()} passengers.");
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
        do
        {
            Console.WriteLine($"Enter the {floorDescription} floor (1 to {totalFloors}):");
            if (int.TryParse(Console.ReadLine(), out int floor) && floor > 0 && floor <= totalFloors)
            {
                return floor;
            }

            Console.WriteLine($"Invalid floor. Please select a floor between 1 and {totalFloors}.");
        }
        while (true);
    }
}