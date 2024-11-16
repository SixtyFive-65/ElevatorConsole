using ElevatorMovement.Services.Implementation;

namespace ElevatorMovement.Services.Interface
{
    public interface IPassenger
    {
        int CurrentFloor { get; }
        int DestinationFloor { get; }
    }
}