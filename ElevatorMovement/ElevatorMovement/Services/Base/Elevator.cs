using ElevatorMovement.Services.Implementation;
using Serilog;

namespace ElevatorMovement.Services.Base
{
    public abstract class Elevator : IElevator
    {
        public int Id { get; }
        public int CurrentFloor { get; private set; }
        private readonly int TotalFloors;
        protected Queue<int> Requests = new Queue<int>();

        protected readonly List<Passenger> Passengers = new List<Passenger>();

        protected readonly List<Passenger> WaitingPassengers = new List<Passenger>();

        private readonly int MaxPassengerCount;  //default : can be made an appsetting

        public Elevator(int id, int totalFloors, int maxPassengerCount)
        {
            Id = id;
            CurrentFloor = 1;
            this.TotalFloors = totalFloors;
            MaxPassengerCount = maxPassengerCount;
        }

        public async Task AddRequest(int floor)
        {
            if (!Requests.Contains(floor))
            {
                Requests.Enqueue(floor);
            }
        }

        public async Task Move()
        {
            try
            {
                while (Requests.Any() || Passengers.Any())
                {
                    if (Requests.Any())
                    {
                        Requests = new Queue<int>(Requests.OrderBy(f => f));

                        int targetFloor = Requests.Dequeue();

                        while (CurrentFloor != targetFloor)
                        {
                            if (CurrentFloor < targetFloor)
                            {
                                CurrentFloor++;
                                LogInformation($"Elevator {Id} moving up to floor {CurrentFloor}");
                            }
                            else if (CurrentFloor > targetFloor)
                            {
                                CurrentFloor--;
                                LogInformation($"Elevator {Id} moving down to floor {CurrentFloor}");
                            }

                            LogInformation($"Elevator {Id} is at floor {CurrentFloor}");

                            Thread.Sleep(2000); // It takes 2 seconds to move to the next floor
                        }

                        await ExitPassengersAtFloor(CurrentFloor);
                        await AddPassengersAtFloor(CurrentFloor);

                        LogInformation($"Elevator {Id} reached floor {CurrentFloor} and opened doors.");
                    }
                }

                LogInformation($"Elevator {Id} is at floor {CurrentFloor}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred. Please try again.");

                Log.Logger.Error(ex?.InnerException?.ToString() ?? ex?.Message);
            }
        }

        public async Task AddPassengersAtFloor(int floor)
        {
            var boardingPassengers = WaitingPassengers.Where(p => p.CurrentFloor == floor).ToList();

            foreach (var passenger in boardingPassengers)
            {
                if (GetPassengerCount() < MaxPassengerCount)
                {
                    Passengers.Add(passenger);
                    WaitingPassengers.Remove(passenger);
                    passenger.HasEnteredElevator = true;
                    LogInformation($"Passenger entered the elevator at floor {floor}. Passenger count: {GetPassengerCount()}");
                }
                else
                {
                    LogInformation($"Elevator is full. Passenger cannot board at floor {floor}. Passenger count: {GetPassengerCount()}");
                }
            }
        }

        public async Task ExitPassengersAtFloor(int floor)
        {
            try
            {
                var exitingPassengers = Passengers.Where(p => p.DestinationFloor == floor).ToList();

                foreach (var passenger in exitingPassengers)
                {
                    Passengers.Remove(passenger);
                    LogInformation($"Passenger exited the elevator at floor {floor}. Passenger count: {GetPassengerCount()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred. Please try again.");

                Log.Logger.Error(ex?.InnerException?.ToString() ?? ex?.Message);
            }
        }

        public int GetPassengerCount()
        {
            return Passengers.Count;
        }

        public async Task RequestElevator(int currentFloor, int destinationFloor)
        {
            try
            {
                await AddRequest(currentFloor);
                await AddRequest(destinationFloor);

                var passenger = new Passenger(currentFloor, destinationFloor);

                // Ensure the passenger only gets added if the elevator has room
                if (GetPassengerCount() < MaxPassengerCount)
                {
                    WaitingPassengers.Add(passenger);
                    LogInformation($"Passenger requested elevator to go from {currentFloor} to {destinationFloor}. Waiting passengers: {WaitingPassengers.Count}");
                }
                else
                {
                    LogInformation($"Elevator is full. Passenger request from {currentFloor} to {destinationFloor} cannot be added.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred. Please try again.");

                Log.Logger.Error(ex?.InnerException?.ToString() ?? ex?.Message);
            }
        }

        public void LogInformation(string message)
        {
            Log.Logger.Information(message);
            Console.WriteLine(message);
        }
    }
}
