using ElevatorMovement.Services.Base;
using ElevatorMovement.Services.Implementation;
using System.Reflection;


public class ElevatorTests
{
    private class TestElevator : Elevator
    {
        public TestElevator(int id, int totalFloors) : base(id, totalFloors) { }

        // Use reflection to get private fields
        public Queue<int> GetRequestsQueue()
        {
            var field = typeof(Elevator).GetField("Requests", BindingFlags.NonPublic | BindingFlags.Instance);
            return (Queue<int>)field.GetValue(this);
        }

        public List<Passenger> GetPassengers()
        {
            var field = typeof(Elevator).GetField("Passengers", BindingFlags.NonPublic | BindingFlags.Instance);
            return (List<Passenger>)field.GetValue(this);
        }

        public List<Passenger> GetWaitingPassengers()
        {
            var field = typeof(Elevator).GetField("WaitingPassengers", BindingFlags.NonPublic | BindingFlags.Instance);
            return (List<Passenger>)field.GetValue(this);
        }
    }

    [Fact]
    public void AddRequest_Should_AddUniqueFloorRequests()
    {
        var elevator = new TestElevator(1, 10);

        elevator.AddRequest(3);
        elevator.AddRequest(5);

        Assert.Equal(2, elevator.GetRequestsQueue().Count);
        Assert.Contains(3, elevator.GetRequestsQueue());
        Assert.Contains(5, elevator.GetRequestsQueue());
    }

    [Fact]
    public void Move_Should_ReachTargetFloor()
    {
        var elevator = new TestElevator(1, 10);

        elevator.AddRequest(5);
        elevator.Move();

        Assert.Equal(5, elevator.CurrentFloor);
    }

    [Fact]
    public void RequestElevator_Should_AddPassengerToWaitingList()
    {
        var elevator = new TestElevator(1, 10);

        elevator.RequestElevator(2, 7);

        var waitingPassengers = elevator.GetWaitingPassengers();
        Assert.Single(waitingPassengers);
        Assert.Equal(2, waitingPassengers[0].CurrentFloor);
        Assert.Equal(7, waitingPassengers[0].DestinationFloor);
    }

    [Fact]
    public void AddPassengersAtFloor_Should_AddWaitingPassengersToElevator()
    {
        var elevator = new TestElevator(1, 10);

        elevator.RequestElevator(2, 7);
        elevator.AddPassengersAtFloor(2);

        Assert.Empty(elevator.GetWaitingPassengers());
        Assert.Single(elevator.GetPassengers());
        Assert.Equal(2, elevator.GetPassengers()[0].CurrentFloor);
        Assert.Equal(7, elevator.GetPassengers()[0].DestinationFloor);
    }

    [Fact]
    public void ExitPassengersAtFloor_Should_RemovePassengersAtDestination()
    {
        var elevator = new TestElevator(1, 10);

        elevator.RequestElevator(1, 5);
        elevator.AddPassengersAtFloor(1);
        elevator.ExitPassengersAtFloor(5);

        Assert.Empty(elevator.GetPassengers());
    }

    [Fact]
    public void GetPassengerCount_Should_ReturnCorrectPassengerCount()
    {
        var elevator = new TestElevator(1, 10);

        elevator.RequestElevator(1, 5);
        elevator.RequestElevator(2, 6);

        Assert.Equal(0, elevator.GetPassengerCount()); // Not on board yet

        elevator.AddPassengersAtFloor(1);
        Assert.Equal(1, elevator.GetPassengerCount());

        elevator.AddPassengersAtFloor(2);
        Assert.Equal(2, elevator.GetPassengerCount());
    }
}
