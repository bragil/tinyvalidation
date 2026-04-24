using BenchmarkDotNet.Attributes;
using TinyValidation.BenchmarkApp.Models;

namespace TinyValidation.BenchmarkApp.Benchmarks;

[MemoryDiagnoser]
public class ValidationBenchmark
{
    private readonly User validUser = new User()
    {
        Id = 1,
        Name = "John Doe",
        Bio = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
        Email = "johndoe@email.com",
        Password = "P@ssw0rd!",
        BirthDate = new DateOnly(1984, 5, 11),
        Active = true,
        CreatedAt = DateTime.Now
    };

    private readonly User invalidUser = new User()
    {
        Id = 1,
        Name = null,
        Bio = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Lorem ipsum dolor sit amet, consectetur adipiscing elit. ",
        Email = "@email.com",
        Password = "aaaaaaaaaaaaaaaa",
        BirthDate = new DateOnly(2027, 5, 11),
        Active = true,
        CreatedAt = DateTime.Now
    };

    [Benchmark]
    public void FluentValidation_valid()
    {
        var validator = new Validators.FluentUserValidator();
        var result = validator.Validate(validUser);
    }

    [Benchmark]
    public void FluentValidation_invalid()
    {
        var validator = new Validators.FluentUserValidator();
        var result = validator.Validate(validUser);
    }

    [Benchmark]
    public void TinyValidation_valid()
    {
        var validator = new Validators.TinyUserValidator();
        var result = validator.Validate(validUser);
    }

    [Benchmark]
    public void TinyValidation_invalid()
    {
        var validator = new Validators.TinyUserValidator();
        var result = validator.Validate(invalidUser);
    }

}
