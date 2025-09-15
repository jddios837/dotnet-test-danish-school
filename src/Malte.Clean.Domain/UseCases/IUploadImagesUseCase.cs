using Malte.Clean.Domain.Common;

namespace Malte.Clean.Domain.UseCases;

public interface IUploadImagesUseCase
{
    Task<ValidationResult> ExecuteAsync(Guid customerId, List<ImageUploadRequest> images);
}

public class ImageUploadRequest
{
    public string Base64Data { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long SizeInBytes { get; set; }
}