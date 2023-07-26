using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestCore.Api.Models.Customer;
using TestCore.Business.CustomerServices;

namespace TestCore.Api.Controllers
{
    [ApiController]
    [Route("api/Customer")]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerServices _customerServices;

        public CustomerController(ICustomerServices customerServices)
        {
            _customerServices = customerServices;
        }

        /// <summary>
        /// Search Customer
        /// </summary>
        /// <returns>List of Customers</returns>
        /// <response code="200">List</response>
        /// <response code="400">Error Message</response>
        [HttpPost]
        [Route("Search")]
        public async Task<IResult> Search(CustomerSearchRequest input)
        {
            var response = new CustomerListResponse { Customers = await _customerServices.Search(input.Name) };
            return Results.Ok(response);
        }
    }
}
