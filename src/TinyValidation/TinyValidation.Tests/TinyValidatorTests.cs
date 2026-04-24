namespace TinyValidation.Tests;

public class TinyValidatorTests
{
    [Fact]
    public void Validate_WithValidUser_ReturnsValidResult()
    {
        var user = new User { Name = "John", Age = 30 };
        var validator = new PersonValidator();

        var result = validator.Validate(user);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_WithInvalidUser_ReturnsErrors()
    {
        var user = new User { Name = "", Age = -1 };
        var validator = new PersonValidator();

        var result = validator.Validate(user);

        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }
}
