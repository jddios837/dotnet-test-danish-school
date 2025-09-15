using Malte.Clean.Domain.Entities;

namespace Malte.Clean.Domain.UseCases;

public interface ICreateCustomerUseCase
{
    Task<Customer> ExecuteAsync(string name, string email, string? phoneNumber = null, string? address = null);
}