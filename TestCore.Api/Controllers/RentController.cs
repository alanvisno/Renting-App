using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TestCore.Api.Exceptions;
using TestCore.Api.Models.Rent;
using TestCore.Business;

namespace TestCore.Api.Controllers
{
    [ApiController]
    [Route("api/Rent")]
    public class RentController : ControllerBase
    {
        private readonly IRentServices _rentServices;
        private readonly IValidator<RentCreateRequest> _createValidator;

        public RentController(IRentServices rentServices, IValidator<RentCreateRequest> createValidator)
        {
            _rentServices = rentServices;
            _createValidator = createValidator;
        }

        /// <summary>
        /// Get Customer Rents
        /// </summary>
        /// <returns>List of Rents</returns>
        /// <response code="200">List</response>
        /// <response code="400">Error Message</response>
        [HttpPost]
        [Route("List")]
        public async Task<IResult> GetList(RentListRequest input)
        {
            var response = new RentListResponse { Rents = await _rentServices.GetRents(input.CustomerId) };
            return Results.Ok(response);
        }

        /// <summary>
        /// Create Rent
        /// </summary>
        /// <returns>Bool</returns>
        /// <response code="200">Success</response>
        /// <response code="401">Customer or Car doesnt exist</response>
        /// <response code="400">Error Message</response>
        [HttpPost]
        [Route("Create")]
        public async Task<IResult> Create(RentCreateRequest input)
        {
            var validationResult = await _createValidator.ValidateAsync(input);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var customer = await _rentServices.GetCustomer(input.CustomerId);
            var car = await _rentServices.GetCar(input.CarId);
            if (customer == null || car == null)
            {
                var errors = new List<string>();
                if (customer == null)
                {
                    errors.Add("Customer with " + input.CustomerId + " could not be found");
                }
                if (car == null)
                {
                    errors.Add("Car with " + input.CarId + " could not be found");
                }
                throw new CustomException("Key not found", errors, HttpStatusCode.NotFound);
            }

            _rentServices.CreateRent(customer, car, input.PricePerDay, input.DeliveryDate, input.ReturnDate);

            return Results.Ok();
        }

        /// <summary>
        /// Return Car
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Error Message</response>
        [HttpPut]
        [Route("Return")]
        public async Task<IResult> Return(RentReturnRequest input)
        {
                var rent = await _rentServices.GetRent(input.RentId);
                if (rent == null)
                {
                    throw new CustomException(
                        "Key not found", 
                        new List<string> { "Rent with " + input.RentId + " could not be found" }, 
                        HttpStatusCode.NotFound);
                }

                _rentServices.ReturnCar(rent, input.ReturnDate);

                return Results.Ok();
        }
    }
}
