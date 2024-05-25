using TestCore.Data.Entities;

namespace TestCore.Business.RentStatus
{
    public class RentStatus
    {
        public IRentStrategy State { get; set; }
        public Rent Rent { get; set; }

        public RentStatus(Rent rent)
        {
            this.State = rent.State switch
            {
                1 => new CancelState(),
                2 => new UsingState(),
                3 => new ReturnedState(),
                _ => throw new Exception("Invalid state."),
            };
            Rent = rent;
        }

        public (bool success, string message) Cancel()
        {
            return State.HandleCancel(this);
        }

        public (bool success, string message) MarkReturned()
        {
            return State.HandleReturned(this);
        }

        public (bool success, string message) MarkUsing()
        {
            return State.HandleUsing(this);
        }
    }
}
