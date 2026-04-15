namespace TinyValidation.Tests;


public class AsyncValidationTests
{

    [Fact]
    public async ValueTask Should_Validate_Simple_Object_Async()
    {
        var client = new Client { Name = "John", Email = "john@gmail.com" };
        var validator = new AsyncUserValidator();
        var result = await validator.ValidateAsync(client);
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async ValueTask Should_Invalidate_Simple_Object_Async()
    {
        var client = new Client { Name = "", Email = "" };
        var validator = new AsyncUserValidator();
        var result = await validator.ValidateAsync(client);
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Equal(3, result.Errors.Count);
    }
}

public struct AsyncUserValidator : ITinyAsyncValidator<Client>
{
    public async ValueTask<ValidationResult> ValidateAsync(Client client)
    {
        return (
            await new Validate()
            .ForAsync(("Name", client.Name),
                      async name => await ValueTask.FromResult(Tiny.NotEmpty(name)), "Name is required.")
            )
            .For(("Name", client.Name),
                name => Tiny.MaxLength(name, 80), "Name must be at most 80 characters.")

            .For(("Email", client.Email),
                email => Tiny.NotEmpty(email), "Email is required.")
            .Run();
    }
}

public class Client
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}