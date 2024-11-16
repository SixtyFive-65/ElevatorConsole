using ElevatorMovement.Services.Base;
using ElevatorMovement.Services.Implementation;
using ElevatorMovement.Services.Interface;
using Serilog;

class Program
{
    static async Task Main()
    {
        ConfigureLogger();
        await RunElevatorApp();
    }

    static async Task RunElevatorApp()
    {
        const int elevatorId = 1;   //we can allow the capture of elevators and floors
        const int totalFloors = 10;

        try
        {
            Log.Logger.Information("Starting Elevator Simulation...");

            IBuilding building = new Building(totalFloors);

            var elevator = new PassengerElevator(elevatorId, totalFloors);  // From Elevator Base, we can create different elevator types

            building.Elevators.Add(elevator);

            while (true)
            {
                var currentFloor = ReadCurrentFloor(totalFloors, "Current");
                var destinationFloor = ReadCurrentFloor(totalFloors, "Destination");

                // Request the elevator
                await elevator.RequestElevator(currentFloor, destinationFloor);

                // Move the elevator and process requests
                foreach (var e in building.Elevators)
                {
                    await e.Move();
                    Log.Logger.Information($"Elevator {e.Id} currently has {((Elevator)e).GetPassengerCount()} passengers.");
                    Console.WriteLine($"Elevator {e.Id} currently has {((Elevator)e).GetPassengerCount()} passengers.");
                }
            }
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex?.InnerException?.ToString() ?? ex?.Message);
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

    static void ConfigureLogger()
    {
        Log.Logger = new LoggerConfiguration()
             .WriteTo.File("C:\\Logs\\elevator_log.txt", rollingInterval: RollingInterval.Day)
             .CreateLogger();
    }
}