using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TestCore.Api.Exceptions;
using TestCore.Api.Models.Rent;
using TestCore.Business;
using TestCore.Data.Entities;

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
        /// <response code="409">The dates matches with other rent</response>
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
                    errors.Add($"Customer with ID {input.CustomerId} could not be found.");
                }
                if (car == null)
                {
                    errors.Add($"Car with ID {input.CarId} could not be found.");
                }
                throw new CustomException("Key not found", errors, HttpStatusCode.NotFound);
            }

            if (_rentServices.HasAlreadyARent(customer.BusinessID, input.DeliveryDate, input.ReturnDate))
            {
                throw new ConflictException("The dates already matches with other rent.");
            }

            await _rentServices.CreateRentAsync(customer, car, input.PricePerDay, input.DeliveryDate, input.ReturnDate);

            return Results.Ok();
        }

        /// <summary>
        /// Return Car
        /// </summary>
        /// <returns>total price</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Error Message</response>
        /// <response code="404">Rent not found</response>
        [HttpPut]
        [Route("Return")]
        public async Task<IResult> Return(RentReturnRequest input)
        {
            var rent = await _rentServices.GetRent(input.RentId) ?? 
                throw new CustomException(
                    "Key not found", 
                    new List<string> { "Rent with Id " + input.RentId + " could not be found" }, 
                    HttpStatusCode.NotFound);

            var premium = new PremiumCustomerDecorator(_rentServices, rent.Customer.Premium);
            var common = new CommonCustomerDecorator(_rentServices, rent.Customer.Premium);
            var total = await premium.ReturnCar(rent, input.ReturnDate) + //in case not, returns 0
                        await common.ReturnCar(rent, input.ReturnDate); //in case not, returns 0

            return Results.Ok(total);
        }
    }
}
