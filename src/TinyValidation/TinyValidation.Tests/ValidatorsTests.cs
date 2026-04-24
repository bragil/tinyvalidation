
namespace TinyValidation.Tests;

public class ValidatorsTests
{
    [Theory]
    [InlineData(null, false)]
    [InlineData("test", true)]
    [InlineData("", true)]
    public void NotNull_ReturnsExpected(string? value, bool expected)
        => Assert.Equal(expected, Tiny.NotNull(value));

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("test", true)]
    public void NotEmpty_WithString_ReturnsExpected(string? value, bool expected)
        => Assert.Equal(expected, Tiny.NotEmpty(value));

    public static TheoryData<List<string>, bool> NotEmpty_CollectionCases => new()
    {
        { new List<string>(), false },
        { new List<string> { "item" }, true }
    };

    [Theory]
    [MemberData(nameof(NotEmpty_CollectionCases))]
    public void NotEmpty_WithCollection_ReturnsExpected(List<string> value, bool expected)
        => Assert.Equal(expected, Tiny.NotEmpty(value));

    [Theory]
    [InlineData("test", 1, 5, true)]
    [InlineData("test", 5, 10, false)]
    [InlineData(null, 1, 5, false)]
    [InlineData("", 1, 5, false)]
    public void Length_ReturnsExpected(string? value, int min, int max, bool expected)
        => Assert.Equal(expected, Tiny.Length(value, min, max));

    [Theory]
    [InlineData("test", 1, true)]
    [InlineData("test", 5, false)]
    [InlineData(null, 1, false)]
    [InlineData("", 1, false)]
    public void MinLength_ReturnsExpected(string? value, int min, bool expected)
        => Assert.Equal(expected, Tiny.MinLength(value, min));

    [Theory]
    [InlineData("test", 5, true)]
    [InlineData("test", 3, false)]
    [InlineData(null, 5, false)]
    [InlineData("", 5, false)]
    public void MaxLength_ReturnsExpected(string? value, int max, bool expected)
        => Assert.Equal(expected, Tiny.MaxLength(value, max));

    [Theory]
    [InlineData("test", 4, true)]
    [InlineData("test", 5, false)]
    [InlineData(null, 4, false)]
    [InlineData("", 4, false)]
    [InlineData("    ", 4, false)] // whitespace-only strings are rejected by the implementation
    public void ExactLength_ReturnsExpected(string? value, int length, bool expected)
        => Assert.Equal(expected, Tiny.ExactLength(value, length));

    [Theory]
    [InlineData(5, 3, true)]
    [InlineData(5, 5, false)]
    [InlineData(5, 10, false)]
    public void GreaterThan_ReturnsExpected(int value, int threshold, bool expected)
        => Assert.Equal(expected, Tiny.GreaterThan(value, threshold));

    [Theory]
    [InlineData(5, 10, true)]
    [InlineData(5, 3, false)]
    [InlineData(5, 5, false)]
    public void LessThan_ReturnsExpected(int value, int threshold, bool expected)
        => Assert.Equal(expected, Tiny.LessThan(value, threshold));

    [Theory]
    [InlineData(5, 1, 10, true)]
    [InlineData(5, 6, 10, false)]
    public void InRange_ReturnsExpected(int value, int min, int max, bool expected)
        => Assert.Equal(expected, Tiny.InRange(value, min, max));

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("test@example.com", true)]
    [InlineData("usuario.dominio.com", false)]
    [InlineData("joao@", false)]
    [InlineData("@empresa.com.br", false)]
    [InlineData("maria@empresa@com.br", false)]
    [InlineData("carlos silva@email.com", false)]
    [InlineData("admin@.com.br", false)]
    [InlineData("teste@dominio..com", false)]
    public void EmailAddress_ReturnsExpected(string? value, bool expected)
        => Assert.Equal(expected, Tiny.EmailAddress(value));

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("     ", false)]
    [InlineData("4035501000000008", true)]  // Visa test
    [InlineData("5066991111111118", true)]  // Elo test
    [InlineData("6771798021000008", true)]  // Maestro test
    [InlineData("5577000055770004", true)]  // Mastercard test
    [InlineData("4000111111311115", false)]
    [InlineData("5105105105105109", false)]
    [InlineData("378736493671000", false)]
    [InlineData("6015000990139424", false)]
    [InlineData("34520000009814", false)]
    public void CreditCard_ReturnsExpected(string? value, bool expected)
        => Assert.Equal(expected, Tiny.CreditCard(value));

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("test", true)]
    [InlineData("test123", false)]
    [InlineData("test!@#", false)]
    public void OnlyLetters_ReturnsExpected(string? value, bool expected)
        => Assert.Equal(expected, Tiny.OnlyLetters(value));

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("test", false)]
    [InlineData("TEST", true)]
    [InlineData("Test", false)]
    [InlineData("12345", false)]
    [InlineData("!@#", false)]
    public void OnlyUpperLetters_ReturnsExpected(string? value, bool expected)
        => Assert.Equal(expected, Tiny.OnlyUpperLetters(value));

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("test", true)]
    [InlineData("TEST", false)]
    [InlineData("Test", false)]
    [InlineData("12345", false)]
    [InlineData("!@#", false)]
    public void OnlyLowerLetters_ReturnsExpected(string? value, bool expected)
        => Assert.Equal(expected, Tiny.OnlyLowerLetters(value));

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("12345", true)]
    [InlineData("test", false)]
    [InlineData("Test12345", false)]
    [InlineData("!@#", false)]
    public void OnlyNumbers_ReturnsExpected(string? value, bool expected)
        => Assert.Equal(expected, Tiny.OnlyNumbers(value));

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("test", true)]
    [InlineData("12345", false)]
    [InlineData("Test12345", true)]
    [InlineData("!@#", false)]
    public void ContainsLetters_ReturnsExpected(string? value, bool expected)
        => Assert.Equal(expected, Tiny.ContainsLetters(value));

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("test", false)]
    [InlineData("TEST", true)]
    [InlineData("Test", true)]
    [InlineData("12345", false)]
    [InlineData("!@#", false)]
    public void ContainsUpperLetters_ReturnsExpected(string? value, bool expected)
        => Assert.Equal(expected, Tiny.ContainsUpperLetters(value));

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("test", true)]
    [InlineData("TEST", false)]
    [InlineData("Test", true)]
    [InlineData("12345", false)]
    [InlineData("!@#", false)]
    public void ContainsLowerLetters_ReturnsExpected(string? value, bool expected)
        => Assert.Equal(expected, Tiny.ContainsLowerLetters(value));

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("12345", true)]
    [InlineData("test", false)]
    [InlineData("Test12345", true)]
    [InlineData("!@#", false)]
    public void ContainsNumbers_ReturnsExpected(string? value, bool expected)
        => Assert.Equal(expected, Tiny.ContainsNumbers(value));

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("test", false)]
    [InlineData("12345", false)]
    [InlineData("Test12345&%$", true)]
    [InlineData("!@#", true)]
    public void ContainsPunctuation_ReturnsExpected(string? value, bool expected)
        => Assert.Equal(expected, Tiny.ContainsPunctuation(value));

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("password", false)]    // no numbers
    [InlineData("123456789", false)]   // no letters
    [InlineData("abc123", false)]      // too short
    [InlineData("abc12345xyz", true)]
    [InlineData("1234567A", true)]
    public void ValidPassword_WithLettersAndNumbers_ReturnsExpected(string? value, bool expected)
        => Assert.Equal(expected, Tiny.ValidPassword(value, 8, 32, RequiredChars.LettersAndNumbers));
}