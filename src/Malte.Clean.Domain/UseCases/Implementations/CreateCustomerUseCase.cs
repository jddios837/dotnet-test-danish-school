using Malte.Clean.Domain.Entities;
using Malte.Clean.Domain.Repositories;

namespace Malte.Clean.Domain.UseCases.Implementations;

public class CreateCustomerUseCase : ICreateCustomerUseCase
{
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerUseCase(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Customer> ExecuteAsync(string name, string email, string? phoneNumber = null, string? address = null)
    {
        // Basic validation
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Customer name cannot be empty", nameof(name));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Customer email cannot be empty", nameof(email));

        // Create new customer entity
        var customer = new Customer(name, email)
        {
            PhoneNumber = phoneNumber,
            Address = address
        };

        try
        {
            return await _customerRepository.CreateAsync(customer);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create customer: {ex.Message}", ex);
        }
    }
}