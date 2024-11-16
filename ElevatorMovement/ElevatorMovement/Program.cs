using ElevatorMovement.Services.Implementation;

class Program
{
    static void Main()
    {
        Building building = new Building(10, 3);
        building.StartSimulation();
    }
}