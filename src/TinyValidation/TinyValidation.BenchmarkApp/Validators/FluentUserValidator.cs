using FluentValidation;
using TinyValidation.BenchmarkApp.Models;

namespace TinyValidation.BenchmarkApp.Validators;

public class FluentUserValidator: AbstractValidator<User>
{
    public FluentUserValidator()
    {
        RuleFor(user => user.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(80).WithMessage("Name must be at most 80 characters.");
        RuleFor(user => user.Bio)
            .MaximumLength(300).WithMessage("Bio must be at most 300 characters.");
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required.")
            .MaximumLength(200).WithMessage("Email must be at most 200 characters.")
            .EmailAddress().WithMessage("Email must be a valid email address.");
        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .MaximumLength(32).WithMessage("Password must be at most 32 characters.")
            .Must(user => Tiny.ContainsLowerLetters(user) 
                            && Tiny.ContainsUpperLetters(user)
                            && Tiny.ContainsNumbers(user)
                            && Tiny.ContainsPunctuation(user))
            .WithMessage("Password must include upper and lower case letters, numbers, and punctuation.");
    }
}
