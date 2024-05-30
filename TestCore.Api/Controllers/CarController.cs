using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TestCore.Api.Exceptions;
using TestCore.Api.Models.Car;
using TestCore.Business;

namespace TestCore.Api.Controllers
{
    [ApiController]
    [Route("api/Car")]
    [Authorize]
    public class CarController : ControllerBase
    {
        private readonly ICarServices _carServices;
        private readonly IValidator<CarCreateRequest> _createValidator;

        public CarController(ICarServices carServices, IValidator<CarCreateRequest> createValidator) 
        {
            _carServices = carServices;
            _createValidator = createValidator;
        }

        /// <summary>
        /// Get Available Cars
        /// </summary>
        /// <returns>List of Cars</returns>
        /// <response code="200">List</response>
        /// <response code="400">Error Message</response>
        [HttpPost]
        [Route("List")]
        public async Task<IResult> List(CarListRequest input)
        {
            var response = new CarListResponse { List = await _carServices.SearchAvailable(input.StartDate, input.EndDate) } ;
            return Results.Ok(response);
        }

        /// <summary>
        /// Create New Car
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Validation Error</response>
        /// <response code="400">Error Message</response>
        [HttpPost]
        [Route("Create")]
        public async Task<IResult> Create(CarCreateRequest input)
        {
            var validationResult = await _createValidator.ValidateAsync(input);
            if (!validationResult.IsValid)
            {
                var messages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                throw new CustomException("Validation Error", messages, HttpStatusCode.BadRequest);
            }

            _carServices.Create(input.Description, input.PricePerDay, input.PatentDate);
            return Results.Ok();
        }
    }
}
