using System;
using System.Threading.Tasks;

namespace TinyValidation;

public static class ValidateExtensions
{
    /// <summary>
    /// Asynchronously validates a property using the provided validation function. 
    /// If the validation fails, an error message is added to the validation results.
    /// </summary>
    /// <typeparam name="TProp">The type of the property value to validate.</typeparam>
    /// <param name="validate">The Validate instance to which the validation results will be added.</param>
    /// <param name="property">A tuple containing the property name and its value to be validated.</param>
    /// <param name="mustAsync">A function that defines the asynchronous condition the property value must satisfy. Returns <see langword="true"/> if the value is valid; otherwise, <see langword="false"/>.</param>
    /// <param name="message">The error message to associate with the property if the validation fails.</param>
    /// <returns>A task that represents the asynchronous validation operation. The task result contains the updated <see cref="Validate"/> instance.</returns>
    public static async ValueTask<Validate> ForAsync<TProp>(this Validate validate,
                                                            (string Name, TProp Value) property,
                                                            Func<TProp, Task<bool>> mustAsync,
                                                            string message)
    {
        if (!await mustAsync(property.Value))
            validate.errors.Add((property.Name, message));

        return validate;
    }

    /// <summary>
    /// Adds a validation rule for a specified property using a custom predicate and error message.
    /// </summary>
    /// <remarks>Use this method to add custom validation logic for individual properties. Multiple calls can
    /// be chained to validate several properties in sequence.</remarks>
    /// <typeparam name="TProp">The type of the property value to validate.</typeparam>
    /// <param name="validate">The validation context to which the rule is added.</param>
    /// <param name="property">A tuple containing the property name and its value to be validated.</param>
    /// <param name="must">A predicate that determines whether the property value satisfies the validation rule. Returns <see
    /// langword="true"/> if the value is valid; otherwise, <see langword="false"/>.</param>
    /// <param name="message">The error message to associate with the property if the validation rule fails.</param>
    /// <returns>The updated validation context, allowing for method chaining.</returns>
    public static Validate For<TProp>(this Validate validate, 
                                      (string Name, TProp Value) property,
                                      Func<TProp, bool> must,
                                      string message)
    {
        if (!must(property.Value))
            validate.errors.Add((property.Name, message));

        return validate;
    }
}
