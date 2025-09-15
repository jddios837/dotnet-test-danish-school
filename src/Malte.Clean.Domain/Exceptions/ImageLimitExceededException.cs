namespace Malte.Clean.Domain.Exceptions;

public class ImageLimitExceededException : ValidationException
{
    public ImageLimitExceededException() : base("Maximum number of images (10) exceeded for this customer")
    {
    }

    public ImageLimitExceededException(int currentCount, int attemptingToAdd)
        : base($"Cannot add {attemptingToAdd} images. Customer already has {currentCount} images. Maximum allowed is 10.")
    {
    }
}