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

        public Elevator(int id, int totalFloors)
        {
            Id = id;
            CurrentFloor = 1;
            this.TotalFloors = totalFloors;
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

                        LogInformation($"Elevator {Id} reached floor {CurrentFloor} and openned doors.");
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
            var boardingPassengers = WaitingPassengers.Where(p => p.CurrentFloor == floor).ToList();  //Would await this call if we called an API or database call

            foreach (var passenger in boardingPassengers)
            {
                Passengers.Add(passenger);
                WaitingPassengers.Remove(passenger);
                passenger.HasEnteredElevator = true;
                LogInformation($"Passenger entered the elevator at floor {floor}. Passenger count: {GetPassengerCount()}");
            }
        }

        public async Task ExitPassengersAtFloor(int floor)
        {
            try
            {
                var exitingPassengers = Passengers.Where(p => p.DestinationFloor == floor).ToList(); //Would await this call if we called an API or database call

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
                WaitingPassengers.Add(passenger);

                LogInformation($"Passenger requested elevator to go from {currentFloor} to {destinationFloor}. Waiting passengers: {WaitingPassengers.Count}");
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
