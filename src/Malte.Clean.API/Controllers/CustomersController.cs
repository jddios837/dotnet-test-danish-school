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
    private readonly ICreateCustomerUseCase _createCustomerUseCase;

    public CustomersController(IGetCustomerUseCase getCustomerUseCase, ICreateCustomerUseCase createCustomerUseCase)
    {
        _getCustomerUseCase = getCustomerUseCase;
        _createCustomerUseCase = createCustomerUseCase;
    }

    /// <summary>
    /// Create a new customer
    /// </summary>
    /// <param name="request">Customer creation details</param>
    /// <returns>Created customer details</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(ApiErrorResponse.BadRequest("Invalid customer data", errors));
        }

        try
        {
            var customer = await _createCustomerUseCase.ExecuteAsync(
                request.Name,
                request.Email,
                request.PhoneNumber,
                request.Address);

            var response = customer.ToResponse();
            return CreatedAtAction(nameof(GetCustomer), new { customerId = customer.Id }, response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiErrorResponse.BadRequest(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiErrorResponse.InternalServerError($"An error occurred while creating customer: {ex.Message}"));
        }
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