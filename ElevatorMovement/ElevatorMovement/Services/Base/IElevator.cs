namespace ElevatorMovement.Services.Base
{
    public interface IElevator
    {
        int Id { get; }
        int CurrentFloor { get; }
        void AddRequest(int floor);
        void Move();
    }
}