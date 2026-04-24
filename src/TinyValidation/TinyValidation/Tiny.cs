using System;
using System.Collections;
using System.Net.Mail;
using System.Numerics;

namespace TinyValidation;

/// <summary>
/// Collection of static methods for performing common validation checks on various types of data.
/// </summary>
public static class Tiny
{
    /// <summary>
    /// Verify if the specified value is not null.
    /// </summary>
    /// <typeparam name="T">The type of the value to check.</typeparam>
    /// <param name="value">The value to check for null.</param>
    /// <returns>true if the value is not null; otherwise, false.</returns>
    public static bool NotNull<T>(T value) 
        => value is not null;

    /// <summary>
    /// Determines whether the specified value is not null and not empty.
    /// </summary>
    /// <param name="value">The value to check for null or emptiness.</param>
    /// <returns>true if the value is not null and not empty; otherwise, false.</returns>
    public static bool NotEmpty<T>(T value)
    {

        if (value is string s && !string.IsNullOrEmpty(s))
            return true;

        if (value is ICollection col && col is not null && col.Count > 0)
            return true;

        if (value is IEnumerable e && e is not null && !IEnumerableIsEmpty(e))
            return true;
        
        return false;
    }

    /// <summary>
    /// Determines whether the length of the specified value is within the given minimum and maximum bounds, inclusive.
    /// </summary>
    /// <remarks>If value is a string, its Length property is used. If value is an ICollection, its Count
    /// property is used. If value is an IEnumerable, the number of elements is counted. If value is not one of these
    /// types, the method returns false.</remarks>
    /// <typeparam name="T">The type of the value to check. Must be a string, an ICollection, or an IEnumerable.</typeparam>
    /// <param name="value">The value whose length is to be validated. Can be a string, an ICollection, or an IEnumerable.</param>
    /// <param name="min">The minimum allowed length, inclusive. Must be less than or equal to max.</param>
    /// <param name="max">The maximum allowed length, inclusive. Must be greater than or equal to min.</param>
    /// <returns>true if the length of value is greater than or equal to min and less than or equal to max; otherwise, false.</returns>
    public static bool Length<T>(T value, int min, int max)
    {
        if (value is null)
            return false;

        if (value is string s)
            return !string.IsNullOrEmpty(s) && s.Length >= min && s.Length <= max;

        if (value is ICollection col)
            return col.Count >= min && col.Count <= max;

        if (value is IEnumerable e)
            return IEnumerableMinLength(e, min) && IEnumerableMaxLength(e, max);

        return false;
    }

    /// <summary>
    /// Determines whether the specified value has a length greater than or equal to the specified minimum length.  
    /// </summary>
    /// <typeparam name="T">The type of the value to check. Can be a string, an ICollection, or an IEnumerable.</typeparam>
    /// <param name="value">The value whose length is to be validated. Can be a string, an ICollection, or an IEnumerable. If the value is
    /// not one of these types, the method returns false.</param>
    /// <param name="minLength">The minimum required length. Must be greater than or equal to zero.</param>
    /// <returns>true if the value's length is greater than or equal to minLength; otherwise, false.</returns>
    public static bool MinLength<T>(T value, int minLength)
    {
        if (value is null)
            return false;

        if (value is string s)
        {
            var span = s.AsSpan();
            return !span.IsEmpty && !span.IsWhiteSpace() && s.Length >= minLength;
        }

        if (value is ICollection col)
            return col.Count >= minLength;

        if (value is IEnumerable e)
            return IEnumerableMinLength(e, minLength);

        return false;
    }

    /// <summary>
    /// Determines whether the specified value does not exceed the given maximum length.
    /// </summary>
    /// <typeparam name="T">The type of the value to check. Typically a string, collection, or enumerable type.</typeparam>
    /// <param name="value">The value whose length or count is to be validated. Can be a string, an object implementing ICollection, or an
    /// object implementing IEnumerable.</param>
    /// <param name="maxLength">The maximum allowed length or count. Must be greater than or equal to zero.</param>
    /// <returns>true if the value's length or count is less than or equal to maxLength; otherwise, false.</returns>
    public static bool MaxLength<T>(T value, int maxLength)
    {
        if (value is null)
            return false;

        if (value is string s)
        {
            var span = s.AsSpan();
            return !span.IsEmpty && !span.IsWhiteSpace() && s.Length <= maxLength;
        }

        if (value is ICollection col)
            return col.Count <= maxLength;

        if (value is IEnumerable e)
            return IEnumerableMaxLength(e, maxLength);

        return false;
    }

    /// <summary>
    /// Determines whether the specified value has exactly the given number of elements or characters.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">The value to check for length. Can be a string, an object implementing ICollection, or an object implementing
    /// IEnumerable.</param>
    /// <param name="length">The exact number of elements or characters to compare against. Must be zero or greater.</param>
    /// <returns>true if the value has exactly the specified number of elements or characters; otherwise, false.</returns>
    public static bool ExactLength<T>(T value, int length)
    {
        if (value is null)
            return false;

        if (value is string s)
        {
            var span = s.AsSpan();
            return !span.IsEmpty && !span.IsWhiteSpace() && s.Length == length;
        }

        if (value is ICollection col)
            return col.Count == length;

        if (value is IEnumerable e)
            return IEnumerableExactLength(e, length);

        return false;
    }

    /// <summary>
    /// Validates that the value is positive , meaning it is greater than the default value for its type. 
    /// For numeric types, this means greater than zero. 
    /// For other types that implement IComparable<T>, it means greater than the default value of that type. 
    /// If the value is null, the method returns false.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool Positive<T>(T value) where T : IComparable<T>
    {
        if (value is null)
            return false;

        return value.CompareTo(default!) > 0;
    }

    /// <summary>
    /// Validates that the value is negative , meaning it is less than the default value for its type. 
    /// For numeric types, this means less than zero. 
    /// For other types that implement IComparable<T>, it means less than the default value of that type. 
    /// If the value is null, the method returns false.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool Negative<T>(T value) where T : IComparable<T>
    {
        if (value is null)
            return false;

        return value.CompareTo(default!) < 0;
    }

    /// <summary>
    /// Validates that the value is greater than the specified threshold.
    /// </summary>
    /// <typeparam name="T">The type of the value to compare. Must implement <see cref="IComparable{T}"/>.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="threshold">The threshold value to compare against.</param>
    /// <returns><see langword="true"/> if the value is greater than the threshold; otherwise, <see langword="false"/>.</returns>
    public static bool GreaterThan<T>(T value, T threshold) where T : IComparable<T>
        => value.CompareTo(threshold) > 0;

    /// <summary>
    /// Determines whether a specified value is less than a given threshold using the default comparer. 
    /// </summary>
    /// <typeparam name="T">The type of objects to compare. Must implement <see cref="IComparable{T}"/>.</typeparam>
    /// <param name="value">The value to compare against the threshold.</param>
    /// <param name="threshold">The threshold value to compare to.</param>
    /// <returns>true if <paramref name="value"/> is less than <paramref name="threshold"/>; otherwise, false.</returns>
    public static bool LessThan<T>(T value, T threshold) where T : IComparable<T>
        => value.CompareTo(threshold) < 0;

    /// <summary>
    /// Determines whether a specified value is within the inclusive range defined by a minimum and maximum value.
    /// </summary>
    /// <remarks>The comparison uses the default comparer for type T as defined by the IComparable<T>
    /// implementation.</remarks>
    /// <typeparam name="T">The type of the values to compare. Must implement <see cref="IComparable{T}"/>.</typeparam>
    /// <param name="value">The value to test for inclusion within the specified range.</param>
    /// <param name="min">The inclusive lower bound of the range.</param>
    /// <param name="max">The inclusive upper bound of the range.</param>
    /// <returns>true if the value is greater than or equal to min and less than or equal to max; otherwise, false.</returns>
    public static bool InRange<T>(T value, T min, T max) where T : IComparable<T>
        => value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;


    /// <summary>
    /// Determines whether the specified string is a valid email address format.
    /// </summary>
    /// <remarks>This method checks the format of the email address but does not verify that the address
    /// exists or is deliverable.</remarks>
    /// <param name="email">The email address string to validate. Cannot be null, empty, or consist only of white-space characters.</param>
    /// <returns>true if the input string is a valid email address format; otherwise, false.</returns>
    public static bool EmailAddress(string? email)
    {
        var span = email.AsSpan();
        if (span.IsEmpty || span.IsWhiteSpace())
            return false;

        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Determines whether the specified credit card number is valid according to the Luhn algorithm.
    /// </summary>
    /// <remarks>This method ignores whitespace characters in the input. Any non-digit, non-whitespace
    /// character will cause the method to return false. The method does not check for card issuer or length validity
    /// beyond the Luhn check.</remarks>
    /// <param name="cardNumber">The credit card number to validate. May include digits and optional whitespace. Cannot be null or empty.</param>
    /// <returns>true if the card number is valid according to the Luhn algorithm; otherwise, false.</returns>
    public static bool CreditCard(string? cardNumber)
    {
        var span = cardNumber.AsSpan();
        if (span.IsEmpty || span.IsWhiteSpace())
            return false;

        int sum = 0;
        bool alternate = false;
        for (int i = span.Length - 1; i >= 0; i--)
        {
            char c = span[i];
            if (char.IsDigit(c))
            {
                int n = c - '0';
                if (alternate)
                {
                    n *= 2;
                    if (n > 9)
                        n -= 9;
                }
                sum += n;
                alternate = !alternate;
            }
            else if (!char.IsWhiteSpace(c))
                return false; // Invalid character found
        }
        return sum % 10 == 0;
    }

    /// <summary>
    /// Determines whether the specified string consists exclusively of letter characters.
    /// </summary>
    /// <param name="value">The string to evaluate. Can be null or empty.</param>
    /// <returns>true if the string contains only letter characters and is not null or empty; otherwise, false.</returns>
    public static bool OnlyLetters(string? value)
    {
        var span = value.AsSpan();
        if (span.IsEmpty || span.IsWhiteSpace())
            return false;

        foreach (char c in span)
        {
            if (!char.IsLetter(c))
                return false;
        }
        return true;
    }

    /// <summary>
    /// Determines whether the specified string consists exclusively of uppercase letter characters.
    /// </summary>
    /// <remarks>This method returns false if the input string is null, empty, or contains any non-uppercase
    /// letter characters, including digits, symbols, or whitespace.</remarks>
    /// <param name="value">The string to evaluate for uppercase letters. Can be null or empty.</param>
    /// <returns>true if the string is not null or empty and every character is an uppercase letter; otherwise, false.</returns>
    public static bool OnlyUpperLetters(string? value)
    {
        var span = value.AsSpan();
        if (span.IsEmpty || span.IsWhiteSpace())
            return false;

        foreach (char c in span)
        {
            if (!char.IsUpper(c))
                return false;
        }
        return true;
    }

    /// <summary>
    /// Determines whether the specified string consists exclusively of lowercase letter characters.
    /// </summary>
    /// <remarks>The method returns false if the input string is null, empty, or contains any character that
    /// is not a lowercase letter as defined by char.IsLower.</remarks>
    /// <param name="value">The string to evaluate. Can be null or empty.</param>
    /// <returns>true if the string contains only lowercase letter characters and is not null or empty; otherwise, false.</returns>
    public static bool OnlyLowerLetters(string? value)
    {
        var span = value.AsSpan();
        if (span.IsEmpty || span.IsWhiteSpace())
            return false;

        foreach (char c in span)
        {
            if (!char.IsLower(c))
                return false;
        }
        return true;
    }

    /// <summary>
    /// Determines whether the specified string consists exclusively of numeric digits.
    /// </summary>
    /// <remarks>This method returns false for null or empty strings. Only characters recognized as digits by
    /// char.IsDigit are considered numeric.</remarks>
    /// <param name="value">The string to evaluate for numeric content. Can be null or empty.</param>
    /// <returns>true if the string contains only digit characters and is not null or empty; otherwise, false.</returns>
    public static bool OnlyNumbers(string? value)
    {
        var span = value.AsSpan();
        if (span.IsEmpty || span.IsWhiteSpace())
            return false;

        foreach (char c in span)
        {
            if (!char.IsDigit(c))
                return false;
        }
        return true;
    }

    /// <summary>
    /// Determines whether the specified string contains at least one letter character.
    /// </summary>
    /// <param name="value">The string to examine for the presence of letter characters. Can be null or empty.</param>
    /// <returns>true if the string contains at least one letter character; otherwise, false.</returns>
    public static bool ContainsLetters(string? value)
    {
        var span = value.AsSpan();
        if (span.IsEmpty || span.IsWhiteSpace())
            return false;

        foreach (char c in span)
        {
            if (char.IsLetter(c))
                return true;
        }
        return false;
    }

    /// <summary>
    /// Determines whether the specified string contains at least one uppercase letter.
    /// </summary>
    /// <param name="value">The string to examine for uppercase letters. Can be null or empty.</param>
    /// <returns>true if the string contains one or more uppercase letters; otherwise, false.</returns>
    public static bool ContainsUpperLetters(string? value)
    {
        var span = value.AsSpan();
        if (span.IsEmpty || span.IsWhiteSpace())
            return false;

        foreach (char c in span)
        {
            if (char.IsUpper(c))
                return true;
        }
        return false;
    }

    /// <summary>
    /// Determines whether the specified string contains at least one lowercase letter.
    /// </summary>
    /// <param name="value">The string to examine for lowercase letters. Can be null or empty.</param>
    /// <returns>true if the string contains one or more lowercase letters; otherwise, false.</returns>
    public static bool ContainsLowerLetters(string? value)
    {
        var span = value.AsSpan();
        if (span.IsEmpty || span.IsWhiteSpace())
            return false;

        foreach (char c in span)
        {
            if (char.IsLower(c))
                return true;
        }
        return false;
    }

    /// <summary>
    /// Determines whether the specified string contains one or more numeric digits.
    /// </summary>
    /// <param name="value">The string to examine for numeric digits. Can be null or empty.</param>
    /// <returns>true if the string contains at least one numeric digit; otherwise, false.</returns>
    public static bool ContainsNumbers(string? value)
    {
        var span = value.AsSpan();
        if (span.IsEmpty || span.IsWhiteSpace())
            return false;

        foreach (char c in span)
        {
            if (char.IsDigit(c))
                return true;
        }
        return false;
    }

    /// <summary>
    /// Determines whether the specified string contains any Unicode punctuation characters.
    /// </summary>
    /// <remarks>Punctuation characters are determined using the Unicode standard as defined by
    /// char.IsPunctuation. If the input string is null or empty, the method returns false.</remarks>
    /// <param name="value">The string to examine for punctuation characters. Can be null or empty.</param>
    /// <returns>true if the string contains at least one punctuation character; otherwise, false.</returns>
    public static bool ContainsPunctuation(string? value)
    {
        var span = value.AsSpan();
        if (span.IsEmpty || span.IsWhiteSpace())
            return false;

        foreach (char c in span)
        {
            if (char.IsPunctuation(c))
                return true;
        }
        return false;
    }

    /// <summary>
    /// Determines whether the specified password meets the required character composition criteria.
    /// </summary>
    /// <remarks>Use this method to enforce password policies based on different combinations of letters,
    /// numbers, and punctuation. The method returns false if the input is null or empty.</remarks>
    /// <param name="value">The password string to validate. Cannot be null or empty.</param>
    /// <param name="requiredChars">A value that specifies the set of character requirements the password must satisfy.</param>
    /// <returns>true if the password meets the specified character requirements; otherwise, false.</returns>
    public static bool ValidPassword(string? value, int minLength, int maxLength, RequiredChars requiredChars)
    {
        var span = value.AsSpan();
        if (span.IsEmpty || span.IsWhiteSpace())
            return false;

        if (minLength > maxLength)
            return false;

        if (span.Length < minLength || span.Length > maxLength)
            return false;

        return requiredChars switch
        {
            RequiredChars.LettersAndNumbers => ContainsLetters(value) 
                                                && ContainsNumbers(value),
            RequiredChars.UpperLowerAndNumbers => ContainsUpperLetters(value) 
                                                    && ContainsLowerLetters(value) 
                                                    && ContainsNumbers(value),
            RequiredChars.UpperLowerNumbersAndPunctuation => ContainsUpperLetters(value) 
                                                            && ContainsLowerLetters(value) 
                                                            && ContainsNumbers(value) 
                                                            && ContainsPunctuation(value),
            _ => false,
        };
    }


    private static bool IEnumerableIsEmpty(IEnumerable enumerable)
    {
        var enumerator = enumerable.GetEnumerator();

        using (enumerator as IDisposable)
        {
            return !enumerator.MoveNext();
        }
    }

    private static bool IEnumerableMinLength(IEnumerable enumerable, int minLength)
    {
        int count = 0;
        var enumerator = enumerable.GetEnumerator();
        using (enumerator as IDisposable)
        {
            while (enumerator.MoveNext())
            {
                count++;
                if (count >= minLength)
                    return true;
            }
        }
        return false;
    }

    private static bool IEnumerableMaxLength(IEnumerable enumerable, int maxLength)
    {
        int count = 0;
        var enumerator = enumerable.GetEnumerator();
        using (enumerator as IDisposable)
        {
            while (enumerator.MoveNext())
            {
                count++;
                if (count > maxLength)
                    return false;
            }
        }
        return true;
    }

    private static bool IEnumerableExactLength(IEnumerable enumerable, int length)
    {
        int count = 0;
        var enumerator = enumerable.GetEnumerator();
        using (enumerator as IDisposable)
        {
            while (enumerator.MoveNext())
            {
                count++;
                if (count != length)
                    return false;
            }
        }
        return true;
    }

}

/// <summary>
/// Specifies the required character sets for validating input values.
/// </summary>
/// <remarks>Use this enumeration to indicate which types of characters must be present in a value, such as a
/// password or user input, to meet validation requirements. The options define combinations of letters, numbers, and
/// punctuation that can be enforced by validation logic.</remarks>
public enum RequiredChars
{
    /// <summary>
    /// Specifies that the value must contain letters and numbers.
    /// </summary>
    LettersAndNumbers,

    /// <summary>
    /// Specifies that the value must contain uppercase letters, lowercase letters, and numbers.
    /// </summary>
    UpperLowerAndNumbers,

    /// <summary>
    /// Specifies a character set that includes uppercase letters, lowercase letters, numbers, and punctuation.
    /// </summary>
    UpperLowerNumbersAndPunctuation
}
