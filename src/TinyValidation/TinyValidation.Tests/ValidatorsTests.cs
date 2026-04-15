
namespace TinyValidation.Tests;

public class ValidatorsTests
{
    [Fact]
    public void Should_Validate_NotNull()
    {
        string? strNull = null;
        string? strValue = "test";
        string? strEmpty = "";

        Assert.False(Tiny.NotNull(strNull));
        Assert.True(Tiny.NotNull(strValue));
        Assert.True(Tiny.NotNull(strEmpty));
    }

    [Fact]
    public void Should_Validate_NotEmpty()
    {
        string? strNull = null;
        string? strValue = "test";
        string? strEmpty = "";
        Assert.False(Tiny.NotEmpty(strNull));
        Assert.True(Tiny.NotEmpty(strValue));
        Assert.False(Tiny.NotEmpty(strEmpty));

        var emptyList = new List<string>();
        var nonEmptyList = new List<string> { "item" };
        Assert.False(Tiny.NotEmpty(emptyList));
        Assert.True(Tiny.NotEmpty(nonEmptyList));
    }

    [Fact]
    public void Should_Validate_Length()
    {
        string str = "test";
        string? strNull = null;
        string strEmpty = "";
        Assert.True(Tiny.Length(str, 1, 5));
        Assert.False(Tiny.Length(str, 5, 10));

        Assert.False(Tiny.Length(strNull, 1, 5));
        Assert.False(Tiny.Length(strEmpty, 1, 5));
    }

    [Fact]
    public void Should_Validate_MinLength()
    {
        string str = "test";
        string? strNull = null;
        string strEmpty = "";
        Assert.True(Tiny.MinLength(str, 1));
        Assert.False(Tiny.MinLength(str, 5));

        Assert.False(Tiny.MinLength(strNull, 1));
        Assert.False(Tiny.MinLength(strEmpty, 1));
    }

    [Fact]
    public void Should_Validate_MaxLength()
    {
        string str = "test";
        string? strNull = null;
        string strEmpty = "";
        Assert.True(Tiny.MaxLength(str, 5));
        Assert.False(Tiny.MaxLength(str, 3));

        Assert.False(Tiny.MaxLength(strNull, 5));
        Assert.False(Tiny.MaxLength(strEmpty, 5));
    }

    [Fact]
    public void Should_Validate_ExactLength()
    {
        string str = "test";
        string? strNull = null;
        string strEmpty = "";
        string strWithSpaces = "    ";
        Assert.True(Tiny.ExactLength(str, 4));
        Assert.False(Tiny.ExactLength(str, 5));

        Assert.False(Tiny.ExactLength(strNull, 4));
        Assert.False(Tiny.ExactLength(strEmpty, 4));
        Assert.True(Tiny.ExactLength(strWithSpaces, 4));
    }

    [Fact]
    public void Should_Validate_GreaterThan()
    {
        int value = 5;
        Assert.True(Tiny.GreaterThan(value, 3));
        Assert.False(Tiny.GreaterThan(value, 5));
    }

    [Fact]
    public void Should_Validate_LessThan()
    {
        int value = 5;
        Assert.True(Tiny.LessThan(value, 10));
        Assert.False(Tiny.LessThan(value, 3));
    }

    [Fact]
    public void Should_Validate_InRange()
    {
        int value = 5;
        Assert.True(Tiny.InRange(value, 1, 10));
        Assert.False(Tiny.InRange(value, 6, 10));
    }

    [Fact]
    public void Should_Validate_Email()
    {
        string? emailNull = null;
        string emailEmpty = "";
        string emailValid = "test@example.com";
        string emailWhiteSpaces = "   ";

        string emailInvalid1 = "usuario.dominio.com";
        string emailInvalid2 = "joao@";
        string emailInvalid3 = "@empresa.com.br";
        string emailInvalid4 = "maria@empresa@com.br";
        string emailInvalid5 = "carlos silva@email.com";
        string emailInvalid6 = "admin@.com.br";
        string emailInvalid7 = "teste@dominio..com";

        Assert.False(Tiny.EmailAddress(emailNull));
        Assert.False(Tiny.EmailAddress(emailEmpty));
        Assert.False(Tiny.EmailAddress(emailWhiteSpaces));
        Assert.True(Tiny.EmailAddress(emailValid));

        Assert.False(Tiny.EmailAddress(emailInvalid1));
        Assert.False(Tiny.EmailAddress(emailInvalid2));
        Assert.False(Tiny.EmailAddress(emailInvalid3));
        Assert.False(Tiny.EmailAddress(emailInvalid4));
        Assert.False(Tiny.EmailAddress(emailInvalid5));
        Assert.False(Tiny.EmailAddress(emailInvalid6));
        Assert.False(Tiny.EmailAddress(emailInvalid7));
    }

    [Fact]
    public void Should_Validate_CreditCard()
    {
        string? cardNull = null;
        string cardEmpty = "";
        string cardWhiteSpaces = "     ";

        // Valid numbers for testing (these are commonly used test card numbers that pass the Luhn check but are not real cards):
        string validCreditCard1 = "4035501000000008"; // Visa (test
        string validCreditCard2 = "5066991111111118"; // Elo (test)
        string validCreditCard3 = "6771798021000008";  // Maestro (test)
        string validCreditCard4 = "5577000055770004"; // Mastercard (test)

        // Invalid numbers (these fail the Luhn check or have incorrect lengths):
        string invalidCreditCard1 = "4000111111311115";
        string invalidCreditCard2 = "5105105105105109";
        string invalidCreditCard3 = "378736493671000";
        string invalidCreditCard4 = "6015000990139424";
        string invalidCreditCard5 = "34520000009814";

        Assert.False(Tiny.CreditCard(cardNull));
        Assert.False(Tiny.CreditCard(cardEmpty));
        Assert.False(Tiny.CreditCard(cardWhiteSpaces));

        Assert.True(Tiny.CreditCard(validCreditCard1));
        Assert.True(Tiny.CreditCard(validCreditCard2));
        Assert.True(Tiny.CreditCard(validCreditCard3));
        Assert.True(Tiny.CreditCard(validCreditCard4));

        Assert.False(Tiny.CreditCard(invalidCreditCard1));
        Assert.False(Tiny.CreditCard(invalidCreditCard2));
        Assert.False(Tiny.CreditCard(invalidCreditCard3));
        Assert.False(Tiny.CreditCard(invalidCreditCard4));
        Assert.False(Tiny.CreditCard(invalidCreditCard5));
    }

    [Fact]
    public void Should_Validate_OnlyLetters()
    {
        string? strNull = null;
        string strEmpty = "";
        string strWithSpaces = "   ";
        string strWithLetters = "test";
        string strWithNumbers = "test123";
        string strWithSpecialChars = "test!@#";

        Assert.False(Tiny.OnlyLetters(strNull));
        Assert.False(Tiny.OnlyLetters(strEmpty));
        Assert.False(Tiny.OnlyLetters(strWithSpaces));
        Assert.True(Tiny.OnlyLetters(strWithLetters));
        Assert.False(Tiny.OnlyLetters(strWithNumbers));
        Assert.False(Tiny.OnlyLetters(strWithSpecialChars));
    }

    [Fact]
    public void Should_Validate_OnlyUpperLetters()
    {
        string? strNull = null;
        string strEmpty = "";
        string strWithSpaces = "   ";
        string strWithLowerLetters = "test";
        string strWithUpperLetters = "TEST";
        string strWithMixedLetters = "Test";
        string strWithNumbers = "12345";
        string strWithSpecialChars = "!@#";

        Assert.False(Tiny.OnlyUpperLetters(strNull));
        Assert.False(Tiny.OnlyUpperLetters(strEmpty));
        Assert.False(Tiny.OnlyUpperLetters(strWithSpaces));
        Assert.False(Tiny.OnlyUpperLetters(strWithLowerLetters));
        Assert.True(Tiny.OnlyUpperLetters(strWithUpperLetters));
        Assert.False(Tiny.OnlyUpperLetters(strWithMixedLetters));
        Assert.False(Tiny.OnlyUpperLetters(strWithNumbers));
        Assert.False(Tiny.OnlyUpperLetters(strWithSpecialChars));
    }

    [Fact]
    public void Should_Validate_OnlyLowerLetters()
    {
        string? strNull = null;
        string strEmpty = "";
        string strWithSpaces = "   ";
        string strWithLowerLetters = "test";
        string strWithUpperLetters = "TEST";
        string strWithMixedLetters = "Test";
        string strWithNumbers = "12345";
        string strWithSpecialChars = "!@#";

        Assert.False(Tiny.OnlyLowerLetters(strNull));
        Assert.False(Tiny.OnlyLowerLetters(strEmpty));
        Assert.False(Tiny.OnlyLowerLetters(strWithSpaces));
        Assert.True(Tiny.OnlyLowerLetters(strWithLowerLetters));
        Assert.False(Tiny.OnlyLowerLetters(strWithUpperLetters));
        Assert.False(Tiny.OnlyLowerLetters(strWithMixedLetters));
        Assert.False(Tiny.OnlyLowerLetters(strWithNumbers));
        Assert.False(Tiny.OnlyLowerLetters(strWithSpecialChars));
    }


    [Fact]
    public void Should_Validate_OnlyNumbers()
    {
        string? strNull = null;
        string strEmpty = "";
        string strWithSpaces = "   ";
        string strWithNumbers = "12345";
        string strWithLetters = "test";
        string strWithMixedLetters = "Test12345";
        string strWithSpecialChars = "!@#";

        Assert.False(Tiny.OnlyNumbers(strNull));
        Assert.False(Tiny.OnlyNumbers(strEmpty));
        Assert.False(Tiny.OnlyNumbers(strWithSpaces));
        Assert.True(Tiny.OnlyNumbers(strWithNumbers));
        Assert.False(Tiny.OnlyNumbers(strWithLetters));
        Assert.False(Tiny.OnlyNumbers(strWithMixedLetters));
        Assert.False(Tiny.OnlyNumbers(strWithSpecialChars));
    }

    [Fact]
    public void Should_Validate_ContainsLetters()
    {
        string? strNull = null;
        string strEmpty = "";
        string strWithSpaces = "   ";
        string strWithLetters = "test";
        string strWithNumbers = "12345";
        string strWithMixedLetters = "Test12345";
        string strWithSpecialChars = "!@#";

        Assert.False(Tiny.ContainsLetters(strNull));
        Assert.False(Tiny.ContainsLetters(strEmpty));
        Assert.False(Tiny.ContainsLetters(strWithSpaces));
        Assert.True(Tiny.ContainsLetters(strWithLetters));
        Assert.False(Tiny.ContainsLetters(strWithNumbers));
        Assert.True(Tiny.ContainsLetters(strWithMixedLetters));
        Assert.False(Tiny.ContainsLetters(strWithSpecialChars));
    }

    [Fact]
    public void Should_Validate_ContainsUpperLetters()
    {
        string? strNull = null;
        string strEmpty = "";
        string strWithSpaces = "   ";
        string strWithLowerLetters = "test";
        string strWithUpperLetters = "TEST";
        string strWithMixedLetters = "Test";
        string strWithNumbers = "12345";
        string strWithSpecialChars = "!@#";

        Assert.False(Tiny.ContainsUpperLetters(strNull));
        Assert.False(Tiny.ContainsUpperLetters(strEmpty));
        Assert.False(Tiny.ContainsUpperLetters(strWithSpaces));
        Assert.False(Tiny.ContainsUpperLetters(strWithLowerLetters));
        Assert.True(Tiny.ContainsUpperLetters(strWithUpperLetters));
        Assert.True(Tiny.ContainsUpperLetters(strWithMixedLetters));
        Assert.False(Tiny.ContainsUpperLetters(strWithNumbers));
        Assert.False(Tiny.ContainsUpperLetters(strWithSpecialChars));
    }

    [Fact]
    public void Should_Validate_ContainsLowerLetters()
    {
        string? strNull = null;
        string strEmpty = "";
        string strWithSpaces = "   ";
        string strWithLowerLetters = "test";
        string strWithUpperLetters = "TEST";
        string strWithMixedLetters = "Test";
        string strWithNumbers = "12345";
        string strWithSpecialChars = "!@#";

        Assert.False(Tiny.ContainsLowerLetters(strNull));
        Assert.False(Tiny.ContainsLowerLetters(strEmpty));
        Assert.False(Tiny.ContainsLowerLetters(strWithSpaces));
        Assert.True(Tiny.ContainsLowerLetters(strWithLowerLetters));
        Assert.False(Tiny.ContainsLowerLetters(strWithUpperLetters));
        Assert.True(Tiny.ContainsLowerLetters(strWithMixedLetters));
        Assert.False(Tiny.ContainsLowerLetters(strWithNumbers));
        Assert.False(Tiny.ContainsLowerLetters(strWithSpecialChars));
    }

    [Fact]
    public void Should_Validate_ContainsNumbers()
    {
        string? strNull = null;
        string strEmpty = "";
        string strWithSpaces = "   ";
        string strWithNumbers = "12345";
        string strWithLetters = "test";
        string strWithMixedLetters = "Test12345";
        string strWithSpecialChars = "!@#";

        Assert.False(Tiny.ContainsNumbers(strNull));
        Assert.False(Tiny.ContainsNumbers(strEmpty));
        Assert.False(Tiny.ContainsNumbers(strWithSpaces));
        Assert.True(Tiny.ContainsNumbers(strWithNumbers));
        Assert.False(Tiny.ContainsNumbers(strWithLetters));
        Assert.True(Tiny.ContainsNumbers(strWithMixedLetters));
        Assert.False(Tiny.ContainsNumbers(strWithSpecialChars));
    }

    [Fact]
    public void Should_Validate_ContainsPunctuation()
    {
        string? strNull = null;
        string strEmpty = "";
        string strWithSpaces = "   ";
        string strWithLetters = "test";
        string strWithNumbers = "12345";
        string strWithMixedLetters = "Test12345&%$";
        string strWithSpecialChars = "!@#";

        Assert.False(Tiny.ContainsPunctuation(strNull));
        Assert.False(Tiny.ContainsPunctuation(strEmpty));
        Assert.False(Tiny.ContainsPunctuation(strWithSpaces));
        Assert.False(Tiny.ContainsPunctuation(strWithLetters));
        Assert.False(Tiny.ContainsPunctuation(strWithNumbers));
        Assert.True(Tiny.ContainsPunctuation(strWithMixedLetters));
        Assert.True(Tiny.ContainsPunctuation(strWithSpecialChars));
    }

    [Fact]
    public void Should_Validate_ValidPassword_LettersAndNumbers()
    {
        string? strNull = null;
        string strEmpty = "";
        string strWithSpaces = "   ";

        Assert.False(Tiny.ValidPassword(strNull, 8, 32, RequiredChars.LettersAndNumbers));
        Assert.False(Tiny.ValidPassword(strEmpty, 8, 32, RequiredChars.LettersAndNumbers));
        Assert.False(Tiny.ValidPassword(strWithSpaces, 8, 32, RequiredChars.LettersAndNumbers));
        Assert.False(Tiny.ValidPassword("password", 8, 32, RequiredChars.LettersAndNumbers)); // No numbers
        Assert.False(Tiny.ValidPassword("123456789", 8, 32, RequiredChars.LettersAndNumbers)); // No letters
        Assert.False(Tiny.ValidPassword("abc123", 8, 32, RequiredChars.LettersAndNumbers)); // Too short
        Assert.True(Tiny.ValidPassword("abc12345xyz", 8, 32, RequiredChars.LettersAndNumbers)); // Valid
        Assert.True(Tiny.ValidPassword("1234567A", 8, 32, RequiredChars.LettersAndNumbers)); // Valid
    }




}