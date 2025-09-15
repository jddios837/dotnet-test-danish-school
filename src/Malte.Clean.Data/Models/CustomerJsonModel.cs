using Malte.Clean.Domain.Entities;

namespace Malte.Clean.Data.Models;

public class CustomerJsonModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public List<CustomerImageJsonModel> Images { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public CustomerJsonModel()
    {
        Images = new List<CustomerImageJsonModel>();
    }

    public static CustomerJsonModel FromDomain(Customer customer)
    {
        return new CustomerJsonModel
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            Address = customer.Address,
            Images = customer.Images.Select(CustomerImageJsonModel.FromDomain).ToList(),
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt
        };
    }

    public Customer ToDomain()
    {
        var customer = new Customer
        {
            Id = Id,
            Name = Name,
            Email = Email,
            PhoneNumber = PhoneNumber,
            Address = Address,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt
        };

        customer.Images.AddRange(Images.Select(img => img.ToDomain()));
        return customer;
    }
}