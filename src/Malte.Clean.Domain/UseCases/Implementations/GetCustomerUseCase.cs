using Malte.Clean.Domain.Entities;
using Malte.Clean.Domain.Repositories;

namespace Malte.Clean.Domain.UseCases.Implementations;

public class GetCustomerUseCase : IGetCustomerUseCase
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerUseCase(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Customer?> ExecuteAsync(Guid customerId)
    {
        try
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            return customer;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to retrieve customer {customerId}", ex);
        }
    }
}