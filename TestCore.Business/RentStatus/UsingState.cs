using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCore.Data.Entities;

namespace TestCore.Business.RentStatus
{
    public class UsingState : IRentStrategy 
    {
        public (bool success, string message) HandleCancel(RentStatus rent)
        {
            rent.State = new CancelState();
            rent.Rent.State = 1;
            return (true, string.Empty);
        }

        public (bool success, string message) HandleReturned(RentStatus rent)
        {
            rent.State = new ReturnedState();
            rent.Rent.State = 3;
            rent.Rent.ReturnLogDate = DateTime.Now;
            return (true, string.Empty);
        }

        public (bool success, string message) HandleCreated(RentStatus rent)
        {
            return (false, "No se puede crear un contrato en uso.");
        }

        public (bool success, string message) HandleUsing(RentStatus rent)
        {
            return (false, "El contrato ya está en uso.");
        }
    }
}
