namespace Malte.Clean.Domain.Entities;

public class CustomerImage
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string Base64Data { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long SizeInBytes { get; set; }
    public DateTime UploadedAt { get; set; }

    public CustomerImage()
    {
        Id = Guid.NewGuid();
        UploadedAt = DateTime.UtcNow;
    }

    public CustomerImage(Guid customerId, string base64Data, string fileName, string contentType, long sizeInBytes) : this()
    {
        CustomerId = customerId;
        Base64Data = base64Data;
        FileName = fileName;
        ContentType = contentType;
        SizeInBytes = sizeInBytes;
    }
}