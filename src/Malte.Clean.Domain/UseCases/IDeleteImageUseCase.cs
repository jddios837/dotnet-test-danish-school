using Malte.Clean.Domain.Common;

namespace Malte.Clean.Domain.UseCases;

public interface IDeleteImageUseCase
{
    Task<ValidationResult> ExecuteAsync(Guid customerId, Guid imageId);
}