using Microsoft.AspNetCore.Mvc;
using Malte.Clean.API.DTOs;
using Malte.Clean.Domain.UseCases;

namespace Malte.Clean.API.Controllers;

[ApiController]
[Route("api/customers")]
[Produces("application/json")]
public class CustomersController : ControllerBase
{
    private readonly IGetCustomerUseCase _getCustomerUseCase;

    public CustomersController(IGetCustomerUseCase getCustomerUseCase)
    {
        _getCustomerUseCase = getCustomerUseCase;
    }

    /// <summary>
    /// Get customer details by ID
    /// </summary>
    /// <param name="customerId">The customer ID</param>
    /// <returns>Customer details including images</returns>
    [HttpGet("{customerId:guid}")]
    [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCustomer(Guid customerId)
    {
        try
        {
            var customer = await _getCustomerUseCase.ExecuteAsync(customerId);

            if (customer == null)
            {
                return NotFound(ApiErrorResponse.NotFound($"Customer with ID '{customerId}' was not found"));
            }

            var response = customer.ToResponse();
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiErrorResponse.InternalServerError($"An error occurred while retrieving customer: {ex.Message}"));
        }
    }
}