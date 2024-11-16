using ElevatorMovement.Services.Implementation;

namespace ElevatorMovement.Services.Interface
{
    public interface IBuilding
    {
        List<Elevator> Elevators { get; }
        int TotalFloors { get; }

        Elevator GetClosestElevator(int floor);
        void StartSimulation();
    }
}