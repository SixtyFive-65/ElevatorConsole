using ElevatorMovement.Services.Interface;

namespace ElevatorMovement.Services.Implementation
{
    public class Passenger : IPassenger
    {
        public int CurrentFloor { get; }
        public int DestinationFloor { get; }
        public bool HasEnteredElevator { get; set; } = false;

        public Passenger(int currentFloor, int destinationFloor)
        {
            CurrentFloor = currentFloor;
            DestinationFloor = destinationFloor;
        }
    }

}
