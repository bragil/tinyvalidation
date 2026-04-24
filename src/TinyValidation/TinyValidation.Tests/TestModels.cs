namespace TinyValidation.Tests;

internal sealed class User
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}

internal readonly struct PersonValidator : ITinyValidator<User>
{
    public ValidationResult Validate(User person) =>
        new Validate()
            .For(("Name", person.Name), name => Tiny.NotEmpty(name), "Name is required.")
            .For(("Name", person.Name), name => Tiny.MaxLength(name, 80), "Name must be at most 80 characters.")
            .For(("Age", person.Age), age => Tiny.Positive(age), "Age must be greater than 0")
            .Run();
}

internal sealed class Client
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

internal readonly struct AsyncClientValidator : ITinyAsyncValidator<Client>
{
    public async ValueTask<ValidationResult> ValidateAsync(Client client) =>
        (await new Validate()
            .ForAsync(
                ("Name", client.Name),
                async name => await ValueTask.FromResult(Tiny.NotEmpty(name)),
                "Name is required."))
        .For(("Name", client.Name), name => Tiny.MaxLength(name, 80), "Name must be at most 80 characters.")
        .For(("Email", client.Email), email => Tiny.NotEmpty(email), "Email is required.")
        .Run();
}
