using Malte.Clean.Domain.Entities;

namespace Malte.Clean.Domain.UseCases;

public interface IGetCustomerUseCase
{
    Task<Customer?> ExecuteAsync(Guid customerId);
}