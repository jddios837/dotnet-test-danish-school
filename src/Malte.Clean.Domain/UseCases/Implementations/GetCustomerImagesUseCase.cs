using Malte.Clean.Domain.Entities;
using Malte.Clean.Domain.Exceptions;
using Malte.Clean.Domain.Repositories;

namespace Malte.Clean.Domain.UseCases.Implementations;

public class GetCustomerImagesUseCase : IGetCustomerImagesUseCase
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerImagesUseCase(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<List<CustomerImage>> ExecuteAsync(Guid customerId)
    {
        // Verify customer exists
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
        {
            throw NotFoundException.ForCustomer(customerId);
        }

        // Get customer images
        try
        {
            var images = await _customerRepository.GetCustomerImagesAsync(customerId);
            return images.OrderBy(img => img.UploadedAt).ToList();
        }
        catch (NotFoundException)
        {
            // Re-throw NotFoundException as is
            throw;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to retrieve images for customer {customerId}", ex);
        }
    }
}