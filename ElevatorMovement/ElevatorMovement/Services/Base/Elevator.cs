using ElevatorMovement.Services.Implementation;

namespace ElevatorMovement.Services.Base
{
    public abstract class Elevator : IElevator
    {
        public int Id { get; }
        public int CurrentFloor { get; private set; }
        private readonly int totalFloors;
        private Queue<int> requests = new Queue<int>();

        private List<Passenger> passengers = new List<Passenger>();

        private List<Passenger> waitingPassengers = new List<Passenger>();

        public Elevator(int id, int totalFloors)
        {
            Id = id;
            CurrentFloor = 1; // Elevator starts on the first floor
            this.totalFloors = totalFloors;
        }

        // Add a floor request (boarding and destination)
        public void AddRequest(int floor)
        {
            if (!requests.Contains(floor))
            {
                requests.Enqueue(floor);
                Console.WriteLine($"Request added for floor {floor}. Queue size: {requests.Count}");
            }
        }

        // Move the elevator with a 5-second delay between floors
        public void Move()
        {
            try
            {
                // Continue moving the elevator until there are no passengers left
                while (requests.Any() || passengers.Any())
                {
                    if (requests.Any())
                    {
                        int targetFloor = requests.Dequeue();  // Get the next target floor
                        Console.WriteLine($"Elevator {Id} moving to floor {targetFloor}. Current floor: {CurrentFloor}");

                        // Move the elevator toward the target floor, one floor at a time
                        while (CurrentFloor != targetFloor)
                        {
                            if (CurrentFloor < targetFloor)
                            {
                                CurrentFloor++;
                                Console.WriteLine($"Elevator {Id} moving up to floor {CurrentFloor}");
                            }
                            else if (CurrentFloor > targetFloor)
                            {
                                CurrentFloor--;
                                Console.WriteLine($"Elevator {Id} moving down to floor {CurrentFloor}");
                            }

                            // Show the current floor
                            Console.WriteLine($"Elevator {Id} is at floor {CurrentFloor}");

                            // Simulate the 5-second delay for each floor moved
                            Thread.Sleep(5000); // 5000 milliseconds = 5 seconds
                        }

                        // Handle passengers exiting or entering at the target floor
                        ExitPassengersAtFloor(CurrentFloor);
                        AddPassengersAtFloor(CurrentFloor);

                        Console.WriteLine($"Elevator {Id} reached floor {CurrentFloor} and opened doors.");
                    }
                }

                Console.WriteLine($"Elevator {Id} has no more passengers to transport.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e?.InnerException?.ToString() ?? e?.Message);
            }
        }

        // Add passengers to the elevator when it reaches a floor
        private void AddPassengersAtFloor(int floor)
        {
            var boardingPassengers = waitingPassengers.Where(p => p.CurrentFloor == floor).ToList();

            foreach (var passenger in boardingPassengers)
            {
                passengers.Add(passenger);  // Add passenger to the elevator
                waitingPassengers.Remove(passenger);  // Remove from the waiting list
                passenger.HasEnteredElevator = true;  // Mark as entered
                Console.WriteLine($"Passenger entered the elevator at floor {floor}. Passenger count: {GetPassengerCount()}");
            }
        }

        // Remove passengers from the elevator when it reaches their destination floor
        private void ExitPassengersAtFloor(int floor)
        {
            var exitingPassengers = passengers.Where(p => p.DestinationFloor == floor).ToList();

            foreach (var passenger in exitingPassengers)
            {
                passengers.Remove(passenger);  // Remove from the elevator
                Console.WriteLine($"Passenger exited the elevator at floor {floor}. Passenger count: {GetPassengerCount()}");
            }
        }

        // Get the number of passengers currently in the elevator
        public int GetPassengerCount()
        {
            return passengers.Count;
        }

        // Method to request the elevator (passenger will be added to waiting list)
        public void RequestElevator(int currentFloor, int destinationFloor)
        {
            AddRequest(currentFloor);
            AddRequest(destinationFloor);

            var passenger = new Passenger(currentFloor, destinationFloor);
            waitingPassengers.Add(passenger);

            Console.WriteLine($"Passenger requested elevator to go from {currentFloor} to {destinationFloor}. Waiting passengers: {waitingPassengers.Count}");
        }
    }
}
   