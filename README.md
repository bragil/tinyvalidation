# tinyvalidation
[![NuGet Version](https://img.shields.io/nuget/v/TinyValidation.svg)](https://www.nuget.org/packages/TinyValidation/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/TinyValidation.svg)](https://www.nuget.org/packages/TinyValidation/)

Extremely lightweight validation library for .NET, with very low CPU and memory consumption.

TinyValidation vs. FluentValidation benchmark results ([here](https://codeberg.org/bragil/tinyvalidation/src/branch/main/src/TinyValidation/TinyValidation.BenchmarkApp)):

![Benchmark](https://codeberg.org/bragil/tinyvalidation/raw/branch/main/misc/benchmark_tinyvalidation.png)

Here is a summary of this benchmark (results may vary depending on the environment):
- Speed (Mean): TinyValidation (102.0 ns) **is approximately 23 times faster than FluentValidation** (2,368.7 ns).
- Memory Allocation (Allocated): The difference here is massive. **TinyValidation allocates only 168 Bytes per validation, compared to a huge 10,272 Bytes from FluentValidation**. This represents a reduction of over 98% in memory allocation.
- Garbage Collector Pressure (Gen0 and Gen1): Due to high memory allocation, FluentValidation triggers the garbage collector frequently (1.6327 in Gen0). TinyValidation, on the other hand, has an almost negligible impact on the GC cycle (only 0.0267 in Gen0 and zero in Gen1).

### Get started

Use the NuGet Package Manager or the CLI to install:

```
dotnet add package TinyValidation
```

### Example

Given the following class:

```csharp
public class Person
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}
```

Create a validator using **TinyValidation** library:

```csharp
public struct PersonValidator : ITinyValidator<Person>
{
    public readonly ValidationResult Validate(Person person)
    {
        return new Validate()
            .For(("Name", person.Name),   // (Property name, property value) 
                name => Tiny.NotEmpty(name), "Name is required")  //  => validation function, error message
            .For(("Name", person.Name), 
                name => Tiny.MaxLength(name, 80), "Name must be at most 80 characters.")
                
            .For(("Age", person.Age), 
                age => age > 0, "Age must be greater than 0")
            .Run();
    }
}
```

### And do validation!

```csharp
var result = new PersonValidator().Validate(person);
if (!result.IsValid)
    return result.Errors; // A List<(string Property, string Error)>

```

### Translate errors to ModelState (Asp.Net Core)

Create a extension method to translate the **TinyValidation** errors to **ModelState**:

```csharp
public static class ValidationResultExtensions
{

    public static void AddToModelState(this ValidationResult validationResult, 
                                       ModelStateDictionary modelState, 
                                       bool clearModelState = false)
    {
        if (clearModelState)
        {
            modelState.Clear();
        }

        // if valid or null/empty list, stop here
        if (validationResult.IsValid || validationResult.Errors is null || validationResult.Errors.Count == 0)
        {
            return;
        }

        // Add errors to ModelState
        foreach (var (property, error) in validationResult.Errors)
        {
            modelState.AddModelError(property, error);
        }
    }
}

// Then...
var result = new PersonValidator().Validate(person);
if (!result.IsValid)
    return result.AddToModelState(ModelState);

```

### A list of ready-to-use function validators:

```csharp
Tiny.NotNull(value)
Tiny.NotEmpty(value)
Tiny.Length(value, min, max)
Tiny.MinLength(value, min)
Tiny.MaxLength(value, max)
Tiny.ExactLength(value, length)
Tiny.Positive(value)
Tiny.Negative(value)
Tiny.GreaterThan(value, threshold)
Tiny.LessThan(value, threshold)
Tiny.InRange(value, min, max)
Tiny.EmailAddress(value)
Tiny.CreditCard(value)
Tiny.OnlyLetters(value)
Tiny.OnlyUpperLetters(value)
Tiny.OnlyLowerLetters(value)
Tiny.OnlyNumbers(value)
Tiny.ContainsLetters(value)
Tiny.ContainsUpperLetters(value)
Tiny.ContainsLowerLetters(value)
Tiny.ContainsNumbers(value)
Tiny.ContainsPunctuation(value)
Tiny.ValidPassword(value, min, max, RequiredChars)
```
