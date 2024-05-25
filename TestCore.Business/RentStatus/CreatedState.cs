using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCore.Data.Entities;

namespace TestCore.Business.RentStatus
{
    public class CreatedState : IRentStrategy 
    {
        public (bool success, string message) HandleCancel(RentStatus rent)
        {
            rent.State = new CancelState();
            rent.Rent.State = 1;
            return (true, string.Empty);
        }

        public (bool success, string message) HandleReturned(RentStatus rent)
        {
            return (false, "The rent was never used.");
        }

        public (bool success, string message) HandleCreated(RentStatus rent)
        {
            return (false, "The rent is already creared.");
        }

        public (bool success, string message) HandleUsing(RentStatus rent)
        {
            rent.State = new UsingState();
            rent.Rent.State = 2;
            return (true, string.Empty);
        }
    }
}
