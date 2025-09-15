using System.ComponentModel.DataAnnotations;

namespace Malte.Clean.API.DTOs;

public class CreateCustomerRequest
{
    [Required]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Name cannot be empty")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Email cannot be empty")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Invalid phone number format")]
    [StringLength(50)]
    public string? PhoneNumber { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }
}