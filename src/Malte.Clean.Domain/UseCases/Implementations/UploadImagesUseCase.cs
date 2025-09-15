using Malte.Clean.Domain.Common;
using Malte.Clean.Domain.Entities;
using Malte.Clean.Domain.Exceptions;
using Malte.Clean.Domain.Repositories;
using Malte.Clean.Domain.Services;

namespace Malte.Clean.Domain.UseCases.Implementations;

public class UploadImagesUseCase : IUploadImagesUseCase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ImageValidationService _imageValidationService;

    public UploadImagesUseCase(ICustomerRepository customerRepository, ImageValidationService imageValidationService)
    {
        _customerRepository = customerRepository;
        _imageValidationService = imageValidationService;
    }

    public async Task<ValidationResult> ExecuteAsync(Guid customerId, List<ImageUploadRequest> images)
    {
        if (images == null || !images.Any())
        {
            return ValidationResult.Failure("At least one image must be provided");
        }

        // Get the customer
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
        {
            throw NotFoundException.ForCustomer(customerId);
        }

        // Validate image count constraint
        var imageCountValidation = _imageValidationService.CanAddImages(customer, images.Count);
        if (imageCountValidation.IsFailure)
        {
            return imageCountValidation;
        }

        // Validate each image
        var validationErrors = new List<string>();
        var validatedImages = new List<CustomerImage>();

        foreach (var imageRequest in images)
        {
            var imageValidation = _imageValidationService.ValidateImage(
                imageRequest.Base64Data,
                imageRequest.FileName,
                imageRequest.ContentType,
                imageRequest.SizeInBytes);

            if (imageValidation.IsFailure)
            {
                validationErrors.AddRange(imageValidation.Errors);
                continue;
            }

            // Create domain entity
            var customerImage = new CustomerImage(
                customerId,
                imageRequest.Base64Data,
                imageRequest.FileName,
                imageRequest.ContentType,
                imageRequest.SizeInBytes);

            validatedImages.Add(customerImage);
        }

        if (validationErrors.Any())
        {
            return ValidationResult.Failure(validationErrors);
        }

        // Add all validated images
        try
        {
            foreach (var image in validatedImages)
            {
                await _customerRepository.AddImageAsync(customerId, image);
            }

            return ValidationResult.Success();
        }
        catch (ImageLimitExceededException ex)
        {
            return ValidationResult.Failure(ex.Message);
        }
        catch (ValidationException ex)
        {
            return ValidationResult.Failure(ex.Errors);
        }
        catch (Exception ex)
        {
            return ValidationResult.Failure($"Failed to upload images: {ex.Message}");
        }
    }
}