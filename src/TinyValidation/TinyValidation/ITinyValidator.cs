using System.Threading.Tasks;

namespace TinyValidation;

/// <summary>
/// Defines a contract for validating instances of a specific type, 
/// providing a method to perform validation and return a result 
/// indicating the validity of the instance and any associated errors.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ITinyValidator<T> where T : class
{
    ValidationResult Validate(T instance);
}

/// <summary>
/// Defines a contract for asynchronously validating instances of a specific type,
/// providing a method to perform validation and return a result
/// indicating the validity of the instance and any associated errors.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ITinyAsyncValidator<T> where T : class
{
    ValueTask<ValidationResult> ValidateAsync(T instance);
}   