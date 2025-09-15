namespace Malte.Clean.Domain.Common;

public class ValidationResult
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public List<string> Errors { get; }

    private ValidationResult(bool isSuccess, List<string> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors ?? new List<string>();
    }

    public static ValidationResult Success()
    {
        return new ValidationResult(true, new List<string>());
    }

    public static ValidationResult Failure(string error)
    {
        return new ValidationResult(false, new List<string> { error });
    }

    public static ValidationResult Failure(List<string> errors)
    {
        return new ValidationResult(false, errors);
    }

    public string GetErrorMessage()
    {
        return string.Join(", ", Errors);
    }
}