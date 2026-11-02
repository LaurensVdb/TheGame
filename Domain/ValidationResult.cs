namespace Domain;

public class ValidationResult
{
    public ValidationResult(bool isValid, string? errorMessage = null)
    {
        IsValid = isValid;
        ErrorMessage = errorMessage;
    }

    public bool IsValid { get; }
    public string? ErrorMessage { get; }
}
