using Microsoft.AspNetCore.Mvc;
using Malte.Clean.API.DTOs;
using Malte.Clean.Domain.Exceptions;
using Malte.Clean.Domain.UseCases;

namespace Malte.Clean.API.Controllers;

[ApiController]
[Route("api/customers/{customerId:guid}/images")]
[Produces("application/json")]
public class CustomerImagesController : ControllerBase
{
    private readonly IUploadImagesUseCase _uploadImagesUseCase;
    private readonly IGetCustomerImagesUseCase _getCustomerImagesUseCase;
    private readonly IDeleteImageUseCase _deleteImageUseCase;

    public CustomerImagesController(
        IUploadImagesUseCase uploadImagesUseCase,
        IGetCustomerImagesUseCase getCustomerImagesUseCase,
        IDeleteImageUseCase deleteImageUseCase)
    {
        _uploadImagesUseCase = uploadImagesUseCase;
        _getCustomerImagesUseCase = getCustomerImagesUseCase;
        _deleteImageUseCase = deleteImageUseCase;
    }

    /// <summary>
    /// Upload one or more images to a customer profile
    /// </summary>
    /// <param name="customerId">The customer ID</param>
    /// <param name="request">The images to upload</param>
    /// <returns>Success or validation errors</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadImages(Guid customerId, [FromBody] UploadImageRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(ApiErrorResponse.BadRequest("Invalid request data", errors));
        }

        try
        {
            var imageUploadRequests = request.Images.ToImageUploadRequests();
            var result = await _uploadImagesUseCase.ExecuteAsync(customerId, imageUploadRequests);

            if (result.IsFailure)
            {
                return BadRequest(ApiErrorResponse.BadRequest("Image upload failed", result.Errors));
            }

            return Created($"/api/customers/{customerId}/images", new { message = "Images uploaded successfully" });
        }
        catch (NotFoundException ex)
        {
            return NotFound(ApiErrorResponse.NotFound(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiErrorResponse.InternalServerError($"An error occurred while uploading images: {ex.Message}"));
        }
    }

    /// <summary>
    /// Get all images for a customer
    /// </summary>
    /// <param name="customerId">The customer ID</param>
    /// <returns>List of customer images</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<CustomerImageResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCustomerImages(Guid customerId)
    {
        try
        {
            var images = await _getCustomerImagesUseCase.ExecuteAsync(customerId);
            var response = images.Select(img => img.ToResponse()).ToList();
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ApiErrorResponse.NotFound(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiErrorResponse.InternalServerError($"An error occurred while retrieving images: {ex.Message}"));
        }
    }

    /// <summary>
    /// Delete a specific image from a customer profile
    /// </summary>
    /// <param name="customerId">The customer ID</param>
    /// <param name="imageId">The image ID to delete</param>
    /// <returns>Success or error response</returns>
    [HttpDelete("{imageId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteImage(Guid customerId, Guid imageId)
    {
        try
        {
            var result = await _deleteImageUseCase.ExecuteAsync(customerId, imageId);

            if (result.IsFailure)
            {
                return BadRequest(ApiErrorResponse.BadRequest("Image deletion failed", result.Errors));
            }

            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ApiErrorResponse.NotFound(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiErrorResponse.InternalServerError($"An error occurred while deleting image: {ex.Message}"));
        }
    }
}