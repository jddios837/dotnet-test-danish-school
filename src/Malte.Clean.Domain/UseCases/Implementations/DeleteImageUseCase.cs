using Malte.Clean.Domain.Common;
using Malte.Clean.Domain.Exceptions;
using Malte.Clean.Domain.Repositories;

namespace Malte.Clean.Domain.UseCases.Implementations;

public class DeleteImageUseCase : IDeleteImageUseCase
{
    private readonly ICustomerRepository _customerRepository;

    public DeleteImageUseCase(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<ValidationResult> ExecuteAsync(Guid customerId, Guid imageId)
    {
        // Verify customer exists
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
        {
            throw NotFoundException.ForCustomer(customerId);
        }

        // Verify image exists for this customer
        var image = await _customerRepository.GetCustomerImageAsync(customerId, imageId);
        if (image == null)
        {
            return ValidationResult.Failure($"Image with ID '{imageId}' was not found for customer '{customerId}'");
        }

        try
        {
            var removed = await _customerRepository.RemoveImageAsync(customerId, imageId);
            if (!removed)
            {
                return ValidationResult.Failure($"Failed to remove image with ID '{imageId}' for customer '{customerId}'");
            }

            return ValidationResult.Success();
        }
        catch (NotFoundException)
        {
            // Re-throw NotFoundException as is
            throw;
        }
        catch (Exception ex)
        {
            return ValidationResult.Failure($"Failed to delete image: {ex.Message}");
        }
    }
}