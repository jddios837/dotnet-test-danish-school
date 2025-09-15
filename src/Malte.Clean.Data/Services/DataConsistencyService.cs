using Malte.Clean.Data.Models;
using Malte.Clean.Domain.Exceptions;

namespace Malte.Clean.Data.Services;

public class DataConsistencyService
{
    public void ValidateCustomerData(CustomerDataContainer dataContainer)
    {
        if (dataContainer == null)
        {
            throw new ValidationException("Data container cannot be null");
        }

        var errors = new List<string>();

        // Check for duplicate customer IDs
        var duplicateCustomerIds = dataContainer.Customers
            .GroupBy(c => c.Id)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateCustomerIds.Any())
        {
            errors.Add($"Duplicate customer IDs found: {string.Join(", ", duplicateCustomerIds)}");
        }

        // Validate each customer
        foreach (var customer in dataContainer.Customers)
        {
            ValidateCustomer(customer, errors);
        }

        if (errors.Any())
        {
            throw new ValidationException(errors);
        }
    }

    private void ValidateCustomer(CustomerJsonModel customer, List<string> errors)
    {
        if (customer.Id == Guid.Empty)
        {
            errors.Add("Customer ID cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(customer.Name))
        {
            errors.Add($"Customer {customer.Id}: Name cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(customer.Email))
        {
            errors.Add($"Customer {customer.Id}: Email cannot be empty");
        }

        // Validate image count constraint
        if (customer.Images.Count > 10)
        {
            errors.Add($"Customer {customer.Id}: Cannot have more than 10 images (current: {customer.Images.Count})");
        }

        // Check for duplicate image IDs within customer
        var duplicateImageIds = customer.Images
            .GroupBy(img => img.Id)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateImageIds.Any())
        {
            errors.Add($"Customer {customer.Id}: Duplicate image IDs found: {string.Join(", ", duplicateImageIds)}");
        }

        // Validate each image
        foreach (var image in customer.Images)
        {
            ValidateCustomerImage(image, customer.Id, errors);
        }
    }

    private void ValidateCustomerImage(CustomerImageJsonModel image, Guid customerId, List<string> errors)
    {
        if (image.Id == Guid.Empty)
        {
            errors.Add($"Customer {customerId}: Image ID cannot be empty");
        }

        if (image.CustomerId != customerId)
        {
            errors.Add($"Customer {customerId}: Image {image.Id} has mismatched customer ID ({image.CustomerId})");
        }

        if (string.IsNullOrWhiteSpace(image.Base64Data))
        {
            errors.Add($"Customer {customerId}: Image {image.Id} has empty Base64 data");
        }

        if (string.IsNullOrWhiteSpace(image.FileName))
        {
            errors.Add($"Customer {customerId}: Image {image.Id} has empty file name");
        }

        if (string.IsNullOrWhiteSpace(image.ContentType))
        {
            errors.Add($"Customer {customerId}: Image {image.Id} has empty content type");
        }

        if (image.SizeInBytes <= 0)
        {
            errors.Add($"Customer {customerId}: Image {image.Id} has invalid size ({image.SizeInBytes})");
        }
    }

    public void EnsureAtomicOperation(CustomerDataContainer originalData, Action operation)
    {
        // Create a backup of the original data
        var backupData = DeepCopyDataContainer(originalData);

        try
        {
            operation();
            ValidateCustomerData(originalData);
        }
        catch
        {
            // Restore the backup if operation fails
            RestoreDataContainer(originalData, backupData);
            throw;
        }
    }

    private CustomerDataContainer DeepCopyDataContainer(CustomerDataContainer original)
    {
        var copy = new CustomerDataContainer();
        foreach (var customer in original.Customers)
        {
            var customerCopy = new CustomerJsonModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address,
                CreatedAt = customer.CreatedAt,
                UpdatedAt = customer.UpdatedAt
            };

            foreach (var image in customer.Images)
            {
                customerCopy.Images.Add(new CustomerImageJsonModel
                {
                    Id = image.Id,
                    CustomerId = image.CustomerId,
                    Base64Data = image.Base64Data,
                    FileName = image.FileName,
                    ContentType = image.ContentType,
                    SizeInBytes = image.SizeInBytes,
                    UploadedAt = image.UploadedAt
                });
            }

            copy.Customers.Add(customerCopy);
        }

        return copy;
    }

    private void RestoreDataContainer(CustomerDataContainer target, CustomerDataContainer backup)
    {
        target.Customers.Clear();
        target.Customers.AddRange(backup.Customers);
    }
}