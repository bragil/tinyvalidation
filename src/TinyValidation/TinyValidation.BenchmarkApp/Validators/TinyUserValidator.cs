using TinyValidation.BenchmarkApp.Models;

namespace TinyValidation.BenchmarkApp.Validators;

public struct TinyUserValidator : ITinyValidator<User>
{
    public ValidationResult Validate(User user)
    {
        return new Validate()
            .For(("Name", user.Name),
                name => Tiny.NotEmpty(name), "Name is required")  
            .For(("Name", user.Name),
                name => Tiny.MaxLength(name, 80), "Name must be at most 80 characters.")

            .For(("Bio", user.Bio),
                bio => Tiny.MaxLength(bio, 300), "Bio must be at most 300 characters.")

            .For(("Email", user.Email),
                email => Tiny.NotEmpty(email), "Email is required")
            .For(("Email", user.Email),
                email => Tiny.MaxLength(email, 200), "Email must be at most 200 characters.")
            .For(("Email", user.Email),
                email => Tiny.EmailAddress(email), "Email must be a valid email address.")

            .For(("Password", user.Password),
                password => Tiny.ValidPassword(password, 8, 32, RequiredChars.UpperLowerNumbersAndPunctuation), 
                            "Password must be between 8 and 32 characters and include upper and lower case letters, numbers, and punctuation.")

            .For(("BirthDate", user.BirthDate),
                birthDate => birthDate < DateOnly.FromDateTime(DateTime.Now), "Birth date must be in the past.")
            .Run();
    }
}