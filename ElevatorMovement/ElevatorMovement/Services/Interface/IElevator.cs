namespace ElevatorMovement.Services.Interface
{
    public interface IElevator
    {
        int CurrentFloor { get; }
        int Id { get; }

        void AddRequest(int floor);
        void Move();
    }
}