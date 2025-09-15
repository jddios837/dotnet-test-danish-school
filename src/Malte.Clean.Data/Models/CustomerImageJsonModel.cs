using Malte.Clean.Domain.Entities;

namespace Malte.Clean.Data.Models;

public class CustomerImageJsonModel
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string Base64Data { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long SizeInBytes { get; set; }
    public DateTime UploadedAt { get; set; }

    public static CustomerImageJsonModel FromDomain(CustomerImage image)
    {
        return new CustomerImageJsonModel
        {
            Id = image.Id,
            CustomerId = image.CustomerId,
            Base64Data = image.Base64Data,
            FileName = image.FileName,
            ContentType = image.ContentType,
            SizeInBytes = image.SizeInBytes,
            UploadedAt = image.UploadedAt
        };
    }

    public CustomerImage ToDomain()
    {
        return new CustomerImage
        {
            Id = Id,
            CustomerId = CustomerId,
            Base64Data = Base64Data,
            FileName = FileName,
            ContentType = ContentType,
            SizeInBytes = SizeInBytes,
            UploadedAt = UploadedAt
        };
    }
}