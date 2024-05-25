using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCore.Data.Entities;

namespace TestCore.Business.RentStatus
{
    public interface IRentStrategy 
    {
        (bool success, string message) HandleCancel(RentStatus rent);
        (bool success, string message) HandleReturned(RentStatus rent);
        (bool success, string message) HandleCreated(RentStatus rent);
        (bool success, string message) HandleUsing(RentStatus rent);
    }
}
