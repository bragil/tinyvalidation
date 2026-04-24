namespace TinyValidation.Tests;

public class AsyncValidationTests
{
    [Fact]
    public async ValueTask ValidateAsync_WithValidClient_ReturnsValidResult()
    {
        var client = new Client { Name = "John", Email = "john@gmail.com" };
        var validator = new AsyncClientValidator();

        var result = await validator.ValidateAsync(client);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async ValueTask ValidateAsync_WithInvalidClient_ReturnsErrors()
    {
        var client = new Client { Name = "", Email = "" };
        var validator = new AsyncClientValidator();

        var result = await validator.ValidateAsync(client);

        Assert.False(result.IsValid);
        Assert.Equal(3, result.Errors.Count);
    }
}