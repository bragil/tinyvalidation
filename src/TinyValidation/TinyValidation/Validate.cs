using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TinyValidation;

/// <summary>
/// Provides a fluent interface for validating object properties and aggregating validation errors.
/// </summary>
/// <remarks>Use this struct to chain multiple property validations and collect any resulting errors. After adding
/// validations with the For method, call Run to obtain a ValidationResult that indicates whether all validations passed
/// and provides details about any errors. This struct is immutable; each validation returns a new instance with the
/// accumulated errors.</remarks>
public readonly struct Validate
{
    internal readonly List<(string Property, string Error)> errors = new();

    public Validate() { }

    /// <summary>
    /// Adds a validation rule for the specified property and records an error if the property value does not satisfy
    /// the given condition.
    /// </summary>
    /// <remarks>Use this method to add custom validation logic for individual properties. Multiple calls can
    /// be chained to validate several properties in sequence.</remarks>
    /// <typeparam name="TProp">The type of the property value to validate.</typeparam>
    /// <param name="property">A tuple containing the property name and its value to be validated.</param>
    /// <param name="must">A predicate that defines the condition the property value must satisfy. Returns <see langword="true"/> if the
    /// value is valid; otherwise, <see langword="false"/>.</param>
    /// <param name="message">The error message to associate with the property if the validation fails.</param>
    /// <returns>The current <see cref="Validate"/> instance to allow method chaining.</returns>
    public readonly Validate For<TProp>((string Name, TProp Value) property,
                                     Func<TProp, bool> must,
                                     string message)
    {
        if (!must(property.Value))
            errors.Add((property.Name, message));

        return this;
    }

    /// <summary>
    /// Executes the validation process and returns the result, including any validation errors encountered.
    /// </summary>
    /// <returns>A ValidationResult object that indicates whether the validation succeeded and contains a collection of
    /// validation errors, if any.</returns>
    public readonly ValidationResult Run()
        => new(errors.Count == 0, errors);
}
