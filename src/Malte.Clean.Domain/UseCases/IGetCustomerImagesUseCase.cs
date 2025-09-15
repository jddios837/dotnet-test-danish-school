using Malte.Clean.Domain.Entities;

namespace Malte.Clean.Domain.UseCases;

public interface IGetCustomerImagesUseCase
{
    Task<List<CustomerImage>> ExecuteAsync(Guid customerId);
}