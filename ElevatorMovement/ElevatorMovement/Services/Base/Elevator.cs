namespace ElevatorMovement.Services.Base
{
    public abstract class Elevator : IElevator
    {
        public int Id { get; }
        public int CurrentFloor { get; private set; }
        private readonly int totalFloors;
        private Queue<int> requests = new Queue<int>();

        public Elevator(int id, int totalFloors)
        {
            Id = id;
            CurrentFloor = 1;
            this.totalFloors = totalFloors;
        }

        public void AddRequest(int floor)
        {
            if (!requests.Contains(floor))
            {
                requests.Enqueue(floor);
            }
        }

        public void Move()
        {
            try
            {
                if (requests.Any())
                {
                    int targetFloor = requests.Peek();
                    if (CurrentFloor < targetFloor)
                    {
                        CurrentFloor++;
                        Console.WriteLine($"Elevator {Id} moving up to floor {CurrentFloor}");
                    }
                    else if (CurrentFloor > targetFloor)
                    {
                        CurrentFloor--;
                        Console.WriteLine($"Elevator {Id} moving down to floor {CurrentFloor}");
                    }

                    if (CurrentFloor == targetFloor)
                    {
                        requests.Dequeue();
                        Console.WriteLine($"Elevator {Id} reached floor {CurrentFloor} and opened doors.");
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e?.InnerException?.ToString() ?? e?.Message);
            }
        }
    }
}
