// =============================================================================
// Comprehensive XML Documentation Tag Examples
// =============================================================================
// This file demonstrates ALL supported XML documentation tags to ensure
// complete coverage for testing MDGen.
// =============================================================================

namespace Example.XmlDocSamples;

// =============================================================================
// LIST TAG EXAMPLES
// =============================================================================

/// <summary>
/// Demonstrates the use of list tags in XML documentation.
/// </summary>
/// <remarks>
/// <para>
/// This class shows how to create various types of lists in documentation.
/// Lists can be bullet lists, numbered lists, or definition lists.
/// </para>
/// <para>
/// Here is an example of a bullet list:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>First bullet item with description</description>
/// </item>
/// <item>
/// <description>Second bullet item with description</description>
/// </item>
/// <item>
/// <description>Third bullet item with description</description>
/// </item>
/// </list>
/// <para>
/// Here is an example of a numbered list:
/// </para>
/// <list type="number">
/// <item>
/// <description>First numbered item</description>
/// </item>
/// <item>
/// <description>Second numbered item</description>
/// </item>
/// <item>
/// <description>Third numbered item</description>
/// </item>
/// </list>
/// <para>
/// Here is an example of a definition list (table):
/// </para>
/// <list type="table">
/// <listheader>
/// <term>Term</term>
/// <description>Definition</description>
/// </listheader>
/// <item>
/// <term>API</term>
/// <description>Application Programming Interface</description>
/// </item>
/// <item>
/// <term>SDK</term>
/// <description>Software Development Kit</description>
/// </item>
/// <item>
/// <term>IDE</term>
/// <description>Integrated Development Environment</description>
/// </item>
/// </list>
/// </remarks>
public class ListExamples
{
    /// <summary>
    /// Gets the supported list types.
    /// </summary>
    /// <returns>
    /// An array of supported list type names:
    /// <list type="bullet">
    /// <item><description>bullet - for unordered lists</description></item>
    /// <item><description>number - for ordered lists</description></item>
    /// <item><description>table - for definition lists</description></item>
    /// </list>
    /// </returns>
    public string[] GetListTypes() => new[] { "bullet", "number", "table" };
}

// =============================================================================
// SEEALSO TAG EXAMPLES
// =============================================================================

/// <summary>
/// Demonstrates the seealso tag for cross-references.
/// </summary>
/// <remarks>
/// The seealso tag is used to indicate related types, members, or external resources
/// that readers might want to consult for additional information.
/// </remarks>
/// <seealso cref="ListExamples"/>
/// <seealso cref="ParagraphExamples"/>
/// <seealso cref="LangwordExamples"/>
/// <seealso href="https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/">XML Documentation Reference</seealso>
public class SeeAlsoExamples
{
    /// <summary>
    /// Gets related documentation.
    /// </summary>
    /// <returns>A string with related info.</returns>
    /// <seealso cref="ListExamples.GetListTypes"/>
    public string GetRelatedDocs() => "See the related types for more information.";
}

// =============================================================================
// PARA TAG EXAMPLES
// =============================================================================

/// <summary>
/// Demonstrates the para tag for creating paragraphs.
/// </summary>
/// <remarks>
/// <para>
/// The para tag is used to add structure to text within summary, remarks,
/// returns, and other documentation tags. It creates paragraph breaks
/// in the rendered documentation.
/// </para>
/// <para>
/// This is a second paragraph. Notice how it appears as a separate block
/// of text in the rendered documentation. This helps organize longer
/// descriptions into readable chunks.
/// </para>
/// <para>
/// A third paragraph can contain <c>inline code</c> and references to
/// other members like <see cref="ListExamples"/>. The para tag is essential
/// for well-formatted, readable documentation.
/// </para>
/// </remarks>
public class ParagraphExamples
{
    /// <summary>
    /// A method with multi-paragraph documentation.
    /// </summary>
    /// <returns>
    /// <para>
    /// The method returns a formatted string.
    /// </para>
    /// <para>
    /// The string contains multiple lines of text that demonstrate
    /// how the para tag works in return value documentation.
    /// </para>
    /// </returns>
    public string GetFormattedText() => "Formatted text content";
}

// =============================================================================
// LANGWORD TAG EXAMPLES
// =============================================================================

/// <summary>
/// Demonstrates the langword tag for C# language keywords.
/// </summary>
/// <remarks>
/// <para>
/// The langword tag is used to mark C# language keywords in documentation.
/// Common uses include <see langword="null"/>, <see langword="true"/>,
/// <see langword="false"/>, <see langword="async"/>, and <see langword="await"/>.
/// </para>
/// <para>
/// When a method can return <see langword="null"/>, it should be documented.
/// Boolean methods often return <see langword="true"/> or <see langword="false"/>.
/// </para>
/// </remarks>
public class LangwordExamples
{
    /// <summary>
    /// Checks if a value is <see langword="null"/>.
    /// </summary>
    /// <param name="value">The value to check. Can be <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the value is <see langword="null"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool IsNull(object? value) => value is null;

    /// <summary>
    /// Gets a value or <see langword="default"/> if not available.
    /// </summary>
    /// <typeparam name="T">The type of value.</typeparam>
    /// <param name="hasValue">Whether the value is available.</param>
    /// <param name="value">The value to return if available.</param>
    /// <returns>
    /// The <paramref name="value"/> if <paramref name="hasValue"/> is <see langword="true"/>;
    /// otherwise, <see langword="default"/>.
    /// </returns>
    public T? GetOrDefault<T>(bool hasValue, T value) => hasValue ? value : default;

    /// <summary>
    /// An <see langword="async"/> method demonstrating langword usage.
    /// </summary>
    /// <returns>A task that completes with <see langword="true"/>.</returns>
    /// <remarks>
    /// This method uses <see langword="async"/> and <see langword="await"/>
    /// to perform asynchronous operations.
    /// </remarks>
    public async Task<bool> DoAsyncWork()
    {
        await Task.Delay(1);
        return true;
    }
}

// =============================================================================
// COMPREHENSIVE TAG USAGE EXAMPLE
// =============================================================================

/// <summary>
/// A comprehensive example using all documentation tags together.
/// </summary>
/// <remarks>
/// <para>
/// This class demonstrates the combined use of multiple documentation tags
/// to create rich, well-structured API documentation.
/// </para>
/// <para>
/// The following features are demonstrated:
/// </para>
/// <list type="bullet">
/// <item><description>Summary and remarks tags</description></item>
/// <item><description>Parameter and return documentation</description></item>
/// <item><description>Exception documentation</description></item>
/// <item><description>Cross-references with see and seealso</description></item>
/// <item><description>Code examples</description></item>
/// <item><description>Lists and paragraphs</description></item>
/// </list>
/// </remarks>
/// <example>
/// <para>Here's how to use this class:</para>
/// <code>
/// var processor = new ComprehensiveTagExample();
/// var result = processor.ProcessData("input", 42);
/// if (result != null)
/// {
///     Console.WriteLine(result);
/// }
/// </code>
/// </example>
/// <seealso cref="ListExamples"/>
/// <seealso cref="ParagraphExamples"/>
/// <seealso cref="LangwordExamples"/>
public class ComprehensiveTagExample
{
    /// <summary>
    /// Processes data with comprehensive documentation.
    /// </summary>
    /// <param name="input">
    /// <para>The input string to process.</para>
    /// <para>Cannot be <see langword="null"/> or empty.</para>
    /// </param>
    /// <param name="multiplier">
    /// The multiplier value. Must be positive.
    /// </param>
    /// <returns>
    /// <para>
    /// The processed result as a string, or <see langword="null"/> if processing fails.
    /// </para>
    /// <para>
    /// The result format depends on the input:
    /// </para>
    /// <list type="bullet">
    /// <item><description>If input is numeric, returns the multiplied value</description></item>
    /// <item><description>If input is text, returns the repeated text</description></item>
    /// </list>
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="input"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="input"/> is empty or whitespace.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="multiplier"/> is less than or equal to zero.
    /// </exception>
    /// <example>
    /// Basic usage:
    /// <code>
    /// var example = new ComprehensiveTagExample();
    /// 
    /// // Process numeric input
    /// var numericResult = example.ProcessData("5", 3);  // Returns "15"
    /// 
    /// // Process text input
    /// var textResult = example.ProcessData("Hi", 2);    // Returns "HiHi"
    /// </code>
    /// </example>
    /// <seealso cref="ProcessDataAsync"/>
    public string? ProcessData(string input, int multiplier)
    {
        ArgumentNullException.ThrowIfNull(input);
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Input cannot be empty", nameof(input));
        if (multiplier <= 0)
            throw new ArgumentOutOfRangeException(nameof(multiplier), "Multiplier must be positive");

        if (int.TryParse(input, out var number))
            return (number * multiplier).ToString();

        return string.Concat(Enumerable.Repeat(input, multiplier));
    }

    /// <summary>
    /// Asynchronously processes data.
    /// </summary>
    /// <param name="input">The input to process.</param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation.
    /// The task result contains the processed data or <see langword="null"/>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method demonstrates <see langword="async"/>/<see langword="await"/> patterns
    /// combined with proper documentation.
    /// </para>
    /// </remarks>
    /// <seealso cref="ProcessData"/>
    public async Task<string?> ProcessDataAsync(string input, CancellationToken cancellationToken = default)
    {
        await Task.Delay(10, cancellationToken);
        return ProcessData(input, 1);
    }
}

// =============================================================================
// VALUE TAG EXAMPLE (for properties)
// =============================================================================

/// <summary>
/// Demonstrates the value tag for property documentation.
/// </summary>
public class ValueTagExample
{
    private string _name = string.Empty;
    private int _count;

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>
    /// A <see cref="string"/> representing the name.
    /// The default value is <see cref="string.Empty"/>.
    /// </value>
    /// <remarks>
    /// <para>
    /// The value tag specifically documents what the property represents,
    /// while summary describes the property itself.
    /// </para>
    /// </remarks>
    public string Name
    {
        get => _name;
        set => _name = value ?? string.Empty;
    }

    /// <summary>
    /// Gets or sets the count.
    /// </summary>
    /// <value>
    /// An <see cref="int"/> representing the count.
    /// Must be non-negative; negative values are clamped to zero.
    /// </value>
    public int Count
    {
        get => _count;
        set => _count = Math.Max(0, value);
    }
}
