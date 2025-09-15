namespace Malte.Clean.Data.Models;

public class CustomerDataContainer
{
    public List<CustomerJsonModel> Customers { get; set; } = new();

    public CustomerDataContainer()
    {
        Customers = new List<CustomerJsonModel>();
    }
}