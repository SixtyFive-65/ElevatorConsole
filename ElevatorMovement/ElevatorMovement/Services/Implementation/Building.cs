using ElevatorMovement.Services.Base;
using ElevatorMovement.Services.Interface;

namespace ElevatorMovement.Services.Implementation
{
    public class Building : IBuilding
    {
        public int TotalFloors { get; }
        public List<IElevator> Elevators { get; }

        public Building(int totalFloors)
        {
            TotalFloors = totalFloors;
            Elevators = new List<IElevator>();
        }

        public IElevator GetClosestElevator(int floor)
        {
            return Elevators.OrderBy(e => Math.Abs(e.CurrentFloor - floor)).First();
        }
    }
}
