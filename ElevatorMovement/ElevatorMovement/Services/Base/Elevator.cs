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
            CurrentFloor = 1; 
            this.totalFloors = totalFloors;
        }

        public void AddRequest(int floor)
        {
            if (!requests.Contains(floor))
            {
                requests.Enqueue(floor);
                Console.WriteLine($"Request added for floor {floor}. Queue size: {requests.Count}");
            }
        }

        public void Move()
        {
            try
            {
                if (requests.Any())
                {
                    int targetFloor = requests.Dequeue();  
                    Console.WriteLine($"Moving to floor {targetFloor}. Current floor: {CurrentFloor}");

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

                    if (CurrentFloor == targetFloor)
                    {
                        ExitPassengersAtFloor(CurrentFloor);

                        AddPassengersAtFloor(CurrentFloor);

                        Console.WriteLine($"Elevator {Id} reached floor {CurrentFloor} and opened doors.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e?.InnerException?.ToString() ?? e?.Message);
            }
        }

        private void AddPassengersAtFloor(int floor)
        {
            var boardingPassengers = waitingPassengers.Where(p => p.CurrentFloor == floor).ToList();

            foreach (var passenger in boardingPassengers)
            {
                passengers.Add(passenger);  
                waitingPassengers.Remove(passenger);  
                passenger.HasEnteredElevator = true;  
                Console.WriteLine($"Passenger entered the elevator at floor {floor}. Passenger count: {GetPassengerCount()}");
            }
        }

        private void ExitPassengersAtFloor(int floor)
        {
            var exitingPassengers = passengers.Where(p => p.DestinationFloor == floor).ToList();

            foreach (var passenger in exitingPassengers)
            {
                passengers.Remove(passenger);  
                Console.WriteLine($"Passenger exited the elevator at floor {floor}. Passenger count: {GetPassengerCount()}");
            }
        }

        public int GetPassengerCount()
        {
            return passengers.Count;
        }

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
