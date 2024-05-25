using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCore.Data.Entities;

namespace TestCore.Business.RentStatus
{
    public class CancelState : IRentStrategy 
    {
        public (bool success, string message) HandleCancel(RentStatus rent)
        {
            return (false, "The rent is already cancel.");
        }

        public (bool success, string message) HandleReturned(RentStatus rent)
        {
            return (false, "The rent is cancel.");
        }

        public (bool success, string message) HandleCreated(RentStatus rent)
        {
            return (false, "The rent is cancel.");
        }

        public (bool success, string message) HandleUsing(RentStatus rent)
        {
            return (false, "The rent is cancel.");
        }
    }
}
