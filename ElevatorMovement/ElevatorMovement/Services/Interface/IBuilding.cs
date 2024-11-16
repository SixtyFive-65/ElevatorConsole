using ElevatorMovement.Services.Base;

namespace ElevatorMovement.Services.Interface
{
    public interface IBuilding
    {
        int TotalFloors { get; }
        List<IElevator> Elevators { get; }
        IElevator GetClosestElevator(int floor);
    }
}