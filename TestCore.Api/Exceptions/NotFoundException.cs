using System.Net;

namespace TestCore.Api.Exceptions
{
    public class NotFoundException : CustomException
    {
        public NotFoundException(string message)
            : base(message, null, HttpStatusCode.NotFound)
        {

        }
    }
}
