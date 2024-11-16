using ElevatorMovement.Services.Implementation;

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

        public void AddRequest(int floor)
        {
            if (!Requests.Contains(floor))
            {
                Requests.Enqueue(floor);
            }
        }

        public void Move()
        {
            try
            {
                while (Requests.Any() || Passengers.Any())
                {
                    if (Requests.Any())
                    {
                        int targetFloor = Requests.Dequeue();  

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

                            Console.WriteLine($"Elevator {Id} is at floor {CurrentFloor}");

                            Thread.Sleep(3000); // It takes 3 seconds to move to the next floor
                        }

                        ExitPassengersAtFloor(CurrentFloor);
                        AddPassengersAtFloor(CurrentFloor);

                        Console.WriteLine($"Elevator {Id} reached floor {CurrentFloor} and opened doors.");
                    }
                }

                Console.WriteLine($"Elevator {Id} is at floor {CurrentFloor}.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e?.InnerException?.ToString() ?? e?.Message);
            }
        }

        public void AddPassengersAtFloor(int floor)
        {
            var boardingPassengers = WaitingPassengers.Where(p => p.CurrentFloor == floor).ToList();

            foreach (var passenger in boardingPassengers)
            {
                Passengers.Add(passenger);
                WaitingPassengers.Remove(passenger);  
                passenger.HasEnteredElevator = true; 
                Console.WriteLine($"Passenger entered the elevator at floor {floor}. Passenger count: {GetPassengerCount()}");
            }
        }

        public void ExitPassengersAtFloor(int floor)
        {
            var exitingPassengers = Passengers.Where(p => p.DestinationFloor == floor).ToList();

            foreach (var passenger in exitingPassengers)
            {
                Passengers.Remove(passenger);
                Console.WriteLine($"Passenger exited the elevator at floor {floor}. Passenger count: {GetPassengerCount()}");
            }
        }

        public int GetPassengerCount()
        {
            return Passengers.Count;
        }

        public void RequestElevator(int currentFloor, int destinationFloor)
        {
            AddRequest(currentFloor);
            AddRequest(destinationFloor);

            var passenger = new Passenger(currentFloor, destinationFloor);
            WaitingPassengers.Add(passenger);

            Console.WriteLine($"Passenger requested elevator to go from {currentFloor} to {destinationFloor}. Waiting passengers: {WaitingPassengers.Count}");
        }
    }
}
   