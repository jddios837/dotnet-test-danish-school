namespace Malte.Clean.API.DTOs;

public class CustomerImageResponse
{
    public Guid Id { get; set; }
    public string Base64Data { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long SizeInBytes { get; set; }
    public DateTime UploadedAt { get; set; }
}