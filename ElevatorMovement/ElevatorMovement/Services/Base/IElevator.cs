namespace ElevatorMovement.Services.Base
{
    public interface IElevator
    {
        int Id { get; }
        int CurrentFloor { get; }
        void AddRequest(int floor);
        void Move();
        void AddPassengersAtFloor(int floor);
        public void ExitPassengersAtFloor(int floor);
    }
}