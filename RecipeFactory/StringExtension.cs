using System.Text.RegularExpressions;

namespace Tools;

/// <summary>
/// Contains extension methods for working with strings.
/// </summary>  
public static partial class StringExtension
{
    #region Regex

    [GeneratedRegex("^[a-z]")]
    private static partial Regex FirstLetterRegex();

	#endregion

	/// <summary>
	/// Changes the case of string's first character to uppercase.
	/// </summary>
	/// <param name="input">Input string.</param>
	/// <returns>Result string.</returns>
	public static string ToFirstUpper(this string input) 
        => string.IsNullOrEmpty(input) ? input : FirstLetterRegex()
            .Replace(input, c => c.Value.ToUpper());
}
