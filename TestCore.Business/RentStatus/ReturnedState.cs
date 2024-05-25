using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCore.Data.Entities;

namespace TestCore.Business.RentStatus
{
    public class ReturnedState : IRentStrategy 
    {
        public (bool success, string message) HandleCancel(RentStatus rent)
        {
            return (false, "The rent can't be cancel because it is finished.");
        }

        public (bool success, string message) HandleReturned(RentStatus rent)
        {
            return (false, "El contrato ya está devuelto.");
        }

        public (bool success, string message) HandleCreated(RentStatus rent)
        {
            return (false, "No se puede marcar como devuelto un contrato recién creado.");
        }

        public (bool success, string message) HandleUsing(RentStatus rent)
        {
            return (false, "No se puede marcar como en uso un contrato devuelto.");
        }
    }
}
