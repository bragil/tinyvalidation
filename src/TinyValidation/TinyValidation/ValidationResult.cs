using System.Collections.Generic;

namespace TinyValidation;

/// <summary>
/// Result of a validation process, containing a boolean indicating if the validation passed and a list of errors if it failed.
/// </summary>
public readonly struct ValidationResult
{
    public bool IsValid { get; }
    public List<(string Property, string Error)> Errors { get; }

    public ValidationResult(bool isValid, List<(string Property, string Error)> errors)
    {
        IsValid = isValid;
        Errors = errors;
    }
}