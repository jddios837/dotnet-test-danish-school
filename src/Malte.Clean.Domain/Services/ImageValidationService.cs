using System.Text.RegularExpressions;
using Malte.Clean.Domain.Common;
using Malte.Clean.Domain.Entities;

namespace Malte.Clean.Domain.Services;

public class ImageValidationService
{
    private const int MaxImagesPerCustomer = 10;
    private const long MaxImageSizeBytes = 5 * 1024 * 1024; // 5MB
    private static readonly string[] AllowedMimeTypes =
    {
        "image/jpeg", "image/jpg", "image/png", "image/gif", "image/bmp", "image/webp"
    };

    public ValidationResult CanAddImages(Customer customer, int newImagesCount)
    {
        var errors = new List<string>();

        if (customer.Images.Count + newImagesCount > MaxImagesPerCustomer)
        {
            errors.Add($"Cannot exceed {MaxImagesPerCustomer} images per customer. Current: {customer.Images.Count}, Attempting to add: {newImagesCount}");
        }

        return errors.Any() ? ValidationResult.Failure(errors) : ValidationResult.Success();
    }

    public ValidationResult ValidateImage(string base64Data, string fileName, string contentType, long sizeInBytes)
    {
        var errors = new List<string>();

        if (sizeInBytes > MaxImageSizeBytes)
        {
            errors.Add($"Image size exceeds maximum allowed size of {MaxImageSizeBytes / (1024 * 1024)}MB");
        }

        if (!AllowedMimeTypes.Contains(contentType.ToLower()))
        {
            errors.Add($"Content type '{contentType}' is not allowed. Allowed types: {string.Join(", ", AllowedMimeTypes)}");
        }

        if (string.IsNullOrWhiteSpace(fileName))
        {
            errors.Add("File name cannot be empty");
        }

        if (!IsValidBase64(base64Data))
        {
            errors.Add("Invalid Base64 format");
        }

        return errors.Any() ? ValidationResult.Failure(errors) : ValidationResult.Success();
    }

    private static bool IsValidBase64(string base64String)
    {
        if (string.IsNullOrWhiteSpace(base64String))
            return false;

        // Remove data URI prefix if present (e.g., "data:image/jpeg;base64,")
        var base64Data = base64String;
        if (base64String.StartsWith("data:"))
        {
            var commaIndex = base64String.IndexOf(',');
            if (commaIndex == -1)
                return false;
            base64Data = base64String.Substring(commaIndex + 1);
        }

        // Check if the string is valid Base64
        try
        {
            Convert.FromBase64String(base64Data);
            return true;
        }
        catch
        {
            return false;
        }
    }
}