using Malte.Clean.Domain.Entities;
using Malte.Clean.Domain.UseCases;

namespace Malte.Clean.API.DTOs;

public static class MappingExtensions
{
    public static CustomerImageResponse ToResponse(this CustomerImage image)
    {
        return new CustomerImageResponse
        {
            Id = image.Id,
            Base64Data = image.Base64Data,
            FileName = image.FileName,
            ContentType = image.ContentType,
            SizeInBytes = image.SizeInBytes,
            UploadedAt = image.UploadedAt
        };
    }

    public static CustomerResponse ToResponse(this Customer customer)
    {
        return new CustomerResponse
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            Address = customer.Address,
            Images = customer.Images.Select(img => img.ToResponse()).ToList(),
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt
        };
    }

    public static ImageUploadRequest ToImageUploadRequest(this ImageUploadDto dto)
    {
        return new ImageUploadRequest
        {
            Base64Data = dto.Base64Data,
            FileName = dto.FileName,
            ContentType = dto.ContentType,
            SizeInBytes = dto.SizeInBytes
        };
    }

    public static List<ImageUploadRequest> ToImageUploadRequests(this List<ImageUploadDto> dtos)
    {
        return dtos.Select(dto => dto.ToImageUploadRequest()).ToList();
    }
}