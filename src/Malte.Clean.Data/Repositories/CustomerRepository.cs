using Malte.Clean.Domain.Entities;
using Malte.Clean.Domain.Exceptions;
using Malte.Clean.Domain.Repositories;
using Malte.Clean.Data.Models;
using Malte.Clean.Data.Storage;
using Malte.Clean.Data.Services;

namespace Malte.Clean.Data.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly JsonStorageService _storageService;
    private readonly DataConsistencyService _consistencyService;
    private const string CustomersFileName = "customers.json";

    public CustomerRepository(JsonStorageService storageService, DataConsistencyService consistencyService)
    {
        _storageService = storageService;
        _consistencyService = consistencyService;
    }

    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        var dataContainer = await GetDataContainerAsync();
        var customerModel = dataContainer.Customers.FirstOrDefault(c => c.Id == id);
        return customerModel?.ToDomain();
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        var dataContainer = await GetDataContainerAsync();
        return dataContainer.Customers.Select(c => c.ToDomain()).ToList();
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        if (customer.Id == Guid.Empty)
        {
            customer.Id = Guid.NewGuid();
        }

        customer.CreatedAt = DateTime.UtcNow;
        customer.UpdatedAt = DateTime.UtcNow;

        var dataContainer = await GetDataContainerAsync();

        // Check if customer already exists
        if (dataContainer.Customers.Any(c => c.Id == customer.Id))
        {
            throw new InvalidOperationException($"Customer with ID {customer.Id} already exists");
        }

        var customerModel = CustomerJsonModel.FromDomain(customer);
        dataContainer.Customers.Add(customerModel);

        await SaveDataContainerAsync(dataContainer);
        return customer;
    }

    public async Task<Customer> UpdateAsync(Customer customer)
    {
        var dataContainer = await GetDataContainerAsync();
        var existingCustomerIndex = dataContainer.Customers.FindIndex(c => c.Id == customer.Id);

        if (existingCustomerIndex == -1)
        {
            throw NotFoundException.ForCustomer(customer.Id);
        }

        customer.UpdatedAt = DateTime.UtcNow;
        var customerModel = CustomerJsonModel.FromDomain(customer);
        dataContainer.Customers[existingCustomerIndex] = customerModel;

        await SaveDataContainerAsync(dataContainer);
        return customer;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var dataContainer = await GetDataContainerAsync();
        var customerToRemove = dataContainer.Customers.FirstOrDefault(c => c.Id == id);

        if (customerToRemove == null)
        {
            return false;
        }

        dataContainer.Customers.Remove(customerToRemove);
        await SaveDataContainerAsync(dataContainer);
        return true;
    }

    public async Task<List<CustomerImage>> GetCustomerImagesAsync(Guid customerId)
    {
        var customer = await GetByIdAsync(customerId);
        if (customer == null)
        {
            throw NotFoundException.ForCustomer(customerId);
        }

        return customer.Images;
    }

    public async Task<CustomerImage?> GetCustomerImageAsync(Guid customerId, Guid imageId)
    {
        var customer = await GetByIdAsync(customerId);
        if (customer == null)
        {
            throw NotFoundException.ForCustomer(customerId);
        }

        return customer.Images.FirstOrDefault(img => img.Id == imageId);
    }

    public async Task AddImageAsync(Guid customerId, CustomerImage image)
    {
        var dataContainer = await GetDataContainerAsync();
        var customerModel = dataContainer.Customers.FirstOrDefault(c => c.Id == customerId);

        if (customerModel == null)
        {
            throw NotFoundException.ForCustomer(customerId);
        }

        // Ensure the image has the correct customer ID
        image.CustomerId = customerId;
        if (image.Id == Guid.Empty)
        {
            image.Id = Guid.NewGuid();
        }
        image.UploadedAt = DateTime.UtcNow;

        var imageModel = CustomerImageJsonModel.FromDomain(image);
        customerModel.Images.Add(imageModel);
        customerModel.UpdatedAt = DateTime.UtcNow;

        await SaveDataContainerAsync(dataContainer);
    }

    public async Task<bool> RemoveImageAsync(Guid customerId, Guid imageId)
    {
        var dataContainer = await GetDataContainerAsync();
        var customerModel = dataContainer.Customers.FirstOrDefault(c => c.Id == customerId);

        if (customerModel == null)
        {
            throw NotFoundException.ForCustomer(customerId);
        }

        var imageToRemove = customerModel.Images.FirstOrDefault(img => img.Id == imageId);
        if (imageToRemove == null)
        {
            return false;
        }

        customerModel.Images.Remove(imageToRemove);
        customerModel.UpdatedAt = DateTime.UtcNow;

        await SaveDataContainerAsync(dataContainer);
        return true;
    }

    private async Task<CustomerDataContainer> GetDataContainerAsync()
    {
        var dataContainer = await _storageService.ReadAsync<CustomerDataContainer>(CustomersFileName);
        var result = dataContainer ?? new CustomerDataContainer();

        // Validate data consistency when loading
        _consistencyService.ValidateCustomerData(result);
        return result;
    }

    private async Task SaveDataContainerAsync(CustomerDataContainer dataContainer)
    {
        // Validate data consistency before saving
        _consistencyService.ValidateCustomerData(dataContainer);
        await _storageService.WriteAsync(CustomersFileName, dataContainer);
    }
}