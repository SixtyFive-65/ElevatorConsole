using ElevatorMovement.Services.Base;

namespace ElevatorMovement.Services.Implementation
{
    public class PassengerElevator : Elevator
    {
        public PassengerElevator(int id, int totalFloors)
            : base(id, totalFloors)
        {
        }
    }
}
