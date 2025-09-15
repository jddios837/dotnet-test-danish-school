using Malte.Clean.Domain.Entities;

namespace Malte.Clean.Domain.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id);
    Task<List<Customer>> GetAllAsync();
    Task<Customer> CreateAsync(Customer customer);
    Task<Customer> UpdateAsync(Customer customer);
    Task<bool> DeleteAsync(Guid id);
    Task<List<CustomerImage>> GetCustomerImagesAsync(Guid customerId);
    Task<CustomerImage?> GetCustomerImageAsync(Guid customerId, Guid imageId);
    Task AddImageAsync(Guid customerId, CustomerImage image);
    Task<bool> RemoveImageAsync(Guid customerId, Guid imageId);
}