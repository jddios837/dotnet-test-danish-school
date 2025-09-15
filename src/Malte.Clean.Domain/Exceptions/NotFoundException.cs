namespace Malte.Clean.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public static NotFoundException ForCustomer(Guid customerId)
    {
        return new NotFoundException($"Customer with ID '{customerId}' was not found");
    }

    public static NotFoundException ForImage(Guid imageId)
    {
        return new NotFoundException($"Image with ID '{imageId}' was not found");
    }

    public static NotFoundException ForCustomerImage(Guid customerId, Guid imageId)
    {
        return new NotFoundException($"Image with ID '{imageId}' was not found for customer '{customerId}'");
    }
}