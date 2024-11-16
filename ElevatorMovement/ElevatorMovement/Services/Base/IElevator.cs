namespace ElevatorMovement.Services.Base
{
    public interface IElevator
    {
        int Id { get; }
        int CurrentFloor { get; }
        Task AddRequest(int floor);
        Task Move();
        Task AddPassengersAtFloor(int floor);
        Task ExitPassengersAtFloor(int floor);
    }
}