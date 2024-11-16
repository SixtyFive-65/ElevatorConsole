using ElevatorMovement.Services.Interface;

namespace ElevatorMovement.Services.Implementation
{
    public class Passenger : IPassenger
    {
        public int CurrentFloor { get; }
        public int DestinationFloor { get; }

        public Passenger(int currentFloor, int destinationFloor)
        {
            CurrentFloor = currentFloor;
            DestinationFloor = destinationFloor;
        }

        public void RequestElevator(Building building)
        {
            Elevator elevator = building.GetClosestElevator(CurrentFloor);
            elevator.AddRequest(CurrentFloor);
            elevator.AddRequest(DestinationFloor);
            Console.WriteLine($"Passenger requested elevator to go from {CurrentFloor} to {DestinationFloor}");
        }
    }
}
