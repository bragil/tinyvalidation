namespace TinyValidation.Tests;

public class TinyValidatorTests
{

    [Fact]
    public void Should_Validate_Simple_Object()
    {
        var person = new User { Name = "John", Age = 30 };
        var validator = new PersonValidator();
        var result = validator.Validate(person);
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_Validate_Simple_Object_Invalid()
    {
        var person = new User { Name = "", Age = -1 };
        var validator = new PersonValidator();
        var result = validator.Validate(person);
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }
}

public struct PersonValidator : ITinyValidator<User>
{
    public readonly ValidationResult Validate(User person)
    {
        return new Validate()
              // (Property name, property value) 
            .For(("Name", person.Name),
                 //  => validation function, error message
                name => Tiny.NotEmpty(name), "Name is required.")
            .For(("Name", person.Name), 
                name => Tiny.MaxLength(name, 80), "Name must be at most 80 characters.")

            .For(("Age", person.Age), 
                age => Tiny.Positive(age), "Age must be greater than 0")
            .Run();
    }
}

public class User
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}
