// =============================================================================
// Modern C# Features with XML Documentation
// =============================================================================

namespace Example.XmlDocSamples;

// =============================================================================
// PATTERN MATCHING
// =============================================================================

/// <summary>
/// Demonstrates pattern matching with documented examples.
/// </summary>
public static class PatternMatchingExamples
{
    /// <summary>
    /// Describes an object using pattern matching.
    /// </summary>
    /// <param name="obj">The object to describe.</param>
    /// <returns>A description of the object based on its type and value.</returns>
    /// <example>
    /// <code>
    /// var result = PatternMatchingExamples.Describe(42);        // "Positive integer: 42"
    /// var result2 = PatternMatchingExamples.Describe("hello"); // "String of length 5"
    /// </code>
    /// </example>
    public static string Describe(object? obj) => obj switch
    {
        null => "null value",
        int i when i > 0 => $"Positive integer: {i}",
        int i when i < 0 => $"Negative integer: {i}",
        int => "Zero",
        string { Length: 0 } => "Empty string",
        string s => $"String of length {s.Length}",
        IEnumerable<int> list => $"Integer collection with {list.Count()} items",
        _ => $"Unknown type: {obj.GetType().Name}"
    };

    /// <summary>
    /// Calculates a discount based on customer type.
    /// </summary>
    /// <param name="customer">The customer record.</param>
    /// <returns>The discount percentage (0-100).</returns>
    public static int CalculateDiscount(CustomerRecord customer) => customer switch
    {
        { IsPremium: true, YearsAsCustomer: > 5 } => 20,
        { IsPremium: true } => 15,
        { YearsAsCustomer: > 10 } => 10,
        { YearsAsCustomer: > 5 } => 5,
        _ => 0
    };
}

/// <summary>
/// A customer record for pattern matching examples.
/// </summary>
/// <param name="Name">The customer name.</param>
/// <param name="IsPremium">Whether the customer has premium status.</param>
/// <param name="YearsAsCustomer">The number of years as a customer.</param>
public record CustomerRecord(string Name, bool IsPremium, int YearsAsCustomer);

// =============================================================================
// NULLABLE REFERENCE TYPES
// =============================================================================

/// <summary>
/// Demonstrates nullable reference type annotations.
/// </summary>
public class NullableExamples
{
    /// <summary>
    /// Gets a required non-null value.
    /// </summary>
    /// <remarks>
    /// This property is guaranteed to never be null after construction.
    /// </remarks>
    public string RequiredValue { get; }

    /// <summary>
    /// Gets or sets an optional nullable value.
    /// </summary>
    /// <remarks>
    /// This property may be null. Always check before use.
    /// </remarks>
    public string? OptionalValue { get; set; }

    /// <summary>
    /// Creates a new instance with the required value.
    /// </summary>
    /// <param name="requiredValue">The required value. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="requiredValue"/> is null.</exception>
    public NullableExamples(string requiredValue)
    {
        RequiredValue = requiredValue ?? throw new ArgumentNullException(nameof(requiredValue));
    }

    /// <summary>
    /// Gets the length of the optional value safely.
    /// </summary>
    /// <returns>The length of <see cref="OptionalValue"/>, or 0 if null.</returns>
    public int GetOptionalLength() => OptionalValue?.Length ?? 0;

    /// <summary>
    /// Tries to get a value from a dictionary.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="dictionary">The dictionary to search.</param>
    /// <param name="key">The key to look up.</param>
    /// <returns>The value if found; otherwise, the default value for <typeparamref name="TValue"/>.</returns>
    public static TValue? GetValueOrDefault<TKey, TValue>(
        IDictionary<TKey, TValue> dictionary,
        TKey key) where TKey : notnull
    {
        return dictionary.TryGetValue(key, out var value) ? value : default;
    }
}

// =============================================================================
// INIT-ONLY AND REQUIRED MEMBERS
// =============================================================================

#if CSHARP11_OR_GREATER
/// <summary>
/// Demonstrates init-only and required properties.
/// </summary>
public class InitOnlyExample
{
    /// <summary>
    /// Gets the ID. Can only be set during initialization.
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// Gets the name. Can only be set during initialization.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the optional description. Can only be set during initialization.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets or sets the mutable status.
    /// </summary>
    public bool IsActive { get; set; }
}
#endif

// =============================================================================
// FILE-SCOPED TYPES (C# 11+)
// =============================================================================

#if CSHARP11_OR_GREATER
/// <summary>
/// A public class that uses file-local helpers.
/// </summary>
public class PublicServiceClass
{
    /// <summary>
    /// Processes data using internal helpers.
    /// </summary>
    /// <param name="input">The input to process.</param>
    /// <returns>The processed result.</returns>
    public string Process(string input)
    {
        return FileLocalHelper.Transform(input);
    }
}

/// <summary>
/// A file-local helper class (only visible in this file).
/// </summary>
file static class FileLocalHelper
{
    /// <summary>
    /// Transforms the input string.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns>The transformed output.</returns>
    public static string Transform(string input) => input.ToUpperInvariant();
}
#endif

// =============================================================================
// COLLECTION EXPRESSIONS (C# 12+)
// =============================================================================

#if CSHARP12_OR_GREATER
/// <summary>
/// Demonstrates collection expressions and patterns.
/// </summary>
public static class CollectionExamples
{
    /// <summary>
    /// Creates a list using collection expression syntax.
    /// </summary>
    /// <returns>A list of integers.</returns>
    public static List<int> CreateList() => [1, 2, 3, 4, 5];

    /// <summary>
    /// Creates an array using collection expression syntax.
    /// </summary>
    /// <returns>An array of strings.</returns>
    public static string[] CreateArray() => ["one", "two", "three"];

    /// <summary>
    /// Spreads multiple collections into one.
    /// </summary>
    /// <param name="first">The first collection.</param>
    /// <param name="second">The second collection.</param>
    /// <returns>A combined list.</returns>
    public static List<int> Combine(int[] first, int[] second) => [.. first, .. second];
}
#endif

// =============================================================================
// RAW STRING LITERALS (C# 11+)
// =============================================================================

#if CSHARP11_OR_GREATER
/// <summary>
/// Demonstrates raw string literals.
/// </summary>
public static class RawStringExamples
{
    /// <summary>
    /// Gets a JSON template using raw string literals.
    /// </summary>
    /// <param name="name">The name to insert.</param>
    /// <param name="value">The value to insert.</param>
    /// <returns>A JSON string.</returns>
    public static string GetJsonTemplate(string name, int value) => $$"""
        {
            "name": "{{name}}",
            "value": {{value}},
            "nested": {
                "array": [1, 2, 3]
            }
        }
        """;

    /// <summary>
    /// Gets a SQL query using raw string literals.
    /// </summary>
    /// <returns>A SQL query string.</returns>
    public static string GetSqlQuery() => """
        SELECT 
            u.Id,
            u.Name,
            u.Email
        FROM Users u
        WHERE u.IsActive = 1
        ORDER BY u.Name
        """;
}
#endif

// =============================================================================
// STATIC ABSTRACT INTERFACE MEMBERS (C# 11+)
// =============================================================================

#if CSHARP11_OR_GREATER
/// <summary>
/// An interface with static abstract members.
/// </summary>
/// <typeparam name="TSelf">The implementing type.</typeparam>
public interface IParsable<TSelf> where TSelf : IParsable<TSelf>
{
    /// <summary>
    /// Parses a string into an instance of the type.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <returns>An instance of <typeparamref name="TSelf"/>.</returns>
    /// <exception cref="FormatException">Thrown when the string cannot be parsed.</exception>
    static abstract TSelf Parse(string s);

    /// <summary>
    /// Tries to parse a string into an instance of the type.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="result">The parsed result if successful.</param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    static abstract bool TryParse(string s, out TSelf? result);
}

/// <summary>
/// A temperature value that implements <see cref="IParsable{TSelf}"/>.
/// </summary>
public readonly struct Temperature : IParsable<Temperature>
{
    /// <summary>
    /// Gets the temperature value in Celsius.
    /// </summary>
    public double Celsius { get; }

    /// <summary>
    /// Creates a new temperature.
    /// </summary>
    /// <param name="celsius">The temperature in Celsius.</param>
    public Temperature(double celsius) => Celsius = celsius;

    /// <summary>
    /// Gets the temperature in Fahrenheit.
    /// </summary>
    public double Fahrenheit => Celsius * 9 / 5 + 32;

    /// <inheritdoc/>
    public static Temperature Parse(string s)
    {
        if (!TryParse(s, out var result))
            throw new FormatException($"Cannot parse '{s}' as Temperature");
        return result;
    }

    /// <inheritdoc/>
    public static bool TryParse(string s, out Temperature result)
    {
        if (double.TryParse(s.TrimEnd('C', 'c', '°'), out var celsius))
        {
            result = new Temperature(celsius);
            return true;
        }
        result = default;
        return false;
    }

    /// <inheritdoc/>
    public override string ToString() => $"{Celsius}°C";
}
#endif

// =============================================================================
// GENERIC MATH (C# 11+)
// =============================================================================

#if NET7_0_OR_GREATER
/// <summary>
/// Demonstrates generic math interfaces.
/// </summary>
public static class GenericMathExamples
{
    /// <summary>
    /// Calculates the sum of a collection using generic math.
    /// </summary>
    /// <typeparam name="T">A numeric type that supports addition.</typeparam>
    /// <param name="values">The values to sum.</param>
    /// <returns>The sum of all values.</returns>
    public static T Sum<T>(IEnumerable<T> values) where T : System.Numerics.INumber<T>
    {
        T sum = T.Zero;
        foreach (var value in values)
            sum += value;
        return sum;
    }

    /// <summary>
    /// Calculates the average of a collection.
    /// </summary>
    /// <typeparam name="T">A numeric type.</typeparam>
    /// <param name="values">The values to average.</param>
    /// <returns>The average value.</returns>
    public static T Average<T>(IEnumerable<T> values) where T : System.Numerics.INumber<T>
    {
        T sum = T.Zero;
        T count = T.Zero;
        foreach (var value in values)
        {
            sum += value;
            count++;
        }
        return sum / count;
    }
}
#endif

// =============================================================================
// DISPOSABLE PATTERNS
// =============================================================================

/// <summary>
/// Demonstrates the disposable pattern with documentation.
/// </summary>
public class DisposableResource : IDisposable, IAsyncDisposable
{
    private bool _disposed;
    private readonly Stream? _stream;

    /// <summary>
    /// Creates a new disposable resource.
    /// </summary>
    /// <param name="stream">The underlying stream to manage.</param>
    public DisposableResource(Stream? stream = null)
    {
        _stream = stream;
    }

    /// <summary>
    /// Performs an operation that requires the resource.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the object has been disposed.</exception>
    public void DoWork()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        // Do work with the stream
    }

    /// <summary>
    /// Releases all resources used by the <see cref="DisposableResource"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Asynchronously releases all resources used by the <see cref="DisposableResource"/>.
    /// </summary>
    /// <returns>A task representing the asynchronous dispose operation.</returns>
    public async ValueTask DisposeAsync()
    {
        if (!_disposed)
        {
            if (_stream is not null)
            {
                await _stream.DisposeAsync();
            }
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release managed resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _stream?.Dispose();
            }
            _disposed = true;
        }
    }
}
