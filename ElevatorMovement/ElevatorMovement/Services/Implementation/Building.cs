using ElevatorMovement.Services.Interface;

namespace ElevatorMovement.Services.Implementation
{
    public class Building : IBuilding
    {
        public int TotalFloors { get; }
        public List<Elevator> Elevators { get; }

        public Building(int totalFloors, int elevatorCount)
        {
            TotalFloors = totalFloors;
            Elevators = new List<Elevator>();
            for (int i = 0; i < elevatorCount; i++)
            {
                Elevators.Add(new Elevator(i + 1, TotalFloors));
            }
        }

        public void StartSimulation()
        {
            Console.WriteLine("Starting Elevator Simulation...");
            while (true)
            {
                Console.WriteLine("Enter current floor");
                string currentFloorInput = Console.ReadLine();
                int.TryParse(currentFloorInput, out var currentFloor);

                Console.WriteLine("Enter current  destination floor");
                string destinationFloorInput = Console.ReadLine();
                int.TryParse(destinationFloorInput, out var destinationFloor);

                Passenger passenger = new Passenger(currentFloor, destinationFloor);
                passenger.RequestElevator(this);

                foreach (var elevator in Elevators)
                {
                    elevator.Move();
                }
            }
        }

        public Elevator GetClosestElevator(int floor)
        {
            return Elevators.OrderBy(e => Math.Abs(e.CurrentFloor - floor)).First();
        }
    }
}
