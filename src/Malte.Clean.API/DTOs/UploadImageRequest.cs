using System.ComponentModel.DataAnnotations;

namespace Malte.Clean.API.DTOs;

public class UploadImageRequest
{
    [Required]
    public List<ImageUploadDto> Images { get; set; } = new();
}

public class ImageUploadDto
{
    [Required]
    [StringLength(int.MaxValue, MinimumLength = 1, ErrorMessage = "Base64Data cannot be empty")]
    public string Base64Data { get; set; } = string.Empty;

    [Required]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "FileName cannot be empty")]
    public string FileName { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^image\/(jpeg|jpg|png|gif|bmp|webp)$", ErrorMessage = "ContentType must be a valid image MIME type")]
    public string ContentType { get; set; } = string.Empty;

    [Range(1, 5 * 1024 * 1024, ErrorMessage = "SizeInBytes must be between 1 and 5MB")]
    public long SizeInBytes { get; set; }
}