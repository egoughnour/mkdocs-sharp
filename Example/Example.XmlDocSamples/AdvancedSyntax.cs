// =============================================================================
// Advanced C# Syntax Elements with XML Documentation
// =============================================================================

namespace Example.XmlDocSamples;

// =============================================================================
// METHODS WITH VARIOUS SIGNATURES
// =============================================================================

/// <summary>
/// Demonstrates various method signatures and documentation patterns.
/// </summary>
public class MethodExamples
{
    /// <summary>
    /// A method with no parameters and no return value.
    /// </summary>
    public void NoParametersNoReturn() { }

    /// <summary>
    /// A method with multiple parameters.
    /// </summary>
    /// <param name="name">The name to greet.</param>
    /// <param name="times">The number of times to repeat the greeting.</param>
    /// <param name="uppercase">Whether to convert to uppercase.</param>
    /// <returns>The formatted greeting string.</returns>
    public string MultipleParameters(string name, int times, bool uppercase = false)
    {
        var greeting = string.Join(" ", Enumerable.Repeat($"Hello, {name}!", times));
        return uppercase ? greeting.ToUpperInvariant() : greeting;
    }

    /// <summary>
    /// A method with an out parameter.
    /// </summary>
    /// <param name="input">The input string to parse.</param>
    /// <param name="result">When this method returns, contains the parsed integer if successful.</param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    public bool TryParse(string input, out int result) => int.TryParse(input, out result);

    /// <summary>
    /// A method with a ref parameter.
    /// </summary>
    /// <param name="value">The value to increment. Modified in place.</param>
    public void IncrementByRef(ref int value) => value++;

    /// <summary>
    /// A method with an in parameter (readonly reference).
    /// </summary>
    /// <param name="point">The point to calculate distance for. Passed by readonly reference.</param>
    /// <returns>The distance from the origin.</returns>
    public double CalculateDistance(in Point2D point) => point.DistanceFromOrigin();

    /// <summary>
    /// A method with params array.
    /// </summary>
    /// <param name="numbers">A variable number of integers to sum.</param>
    /// <returns>The sum of all provided numbers.</returns>
    public int Sum(params int[] numbers) => numbers.Sum();

    /// <summary>
    /// A generic method with constraints.
    /// </summary>
    /// <typeparam name="T">The type of items to compare. Must implement <see cref="IComparable{T}"/>.</typeparam>
    /// <param name="a">The first item.</param>
    /// <param name="b">The second item.</param>
    /// <returns>The larger of the two items.</returns>
    public T Max<T>(T a, T b) where T : IComparable<T> => a.CompareTo(b) >= 0 ? a : b;

    /// <summary>
    /// An async method that returns a task.
    /// </summary>
    /// <param name="url">The URL to fetch.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response content.</returns>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
    public async Task<string> FetchDataAsync(string url, CancellationToken cancellationToken = default)
    {
        using var client = new HttpClient();
        return await client.GetStringAsync(url, cancellationToken);
    }

    /// <summary>
    /// An async method returning ValueTask for better performance.
    /// </summary>
    /// <param name="key">The cache key.</param>
    /// <returns>A value task containing the cached value or null.</returns>
    public ValueTask<string?> GetCachedValueAsync(string key)
    {
        // Simulated cache hit - returns synchronously
        return ValueTask.FromResult<string?>(null);
    }

    /// <summary>
    /// A method that yields values lazily.
    /// </summary>
    /// <param name="start">The starting value.</param>
    /// <param name="count">The number of values to generate.</param>
    /// <returns>An enumerable sequence of integers.</returns>
    public IEnumerable<int> GenerateSequence(int start, int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return start + i;
        }
    }

    /// <summary>
    /// An async enumerable method for streaming data.
    /// </summary>
    /// <param name="count">The number of items to stream.</param>
    /// <param name="delayMs">The delay between items in milliseconds.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>An async enumerable of integers.</returns>
    public async IAsyncEnumerable<int> StreamDataAsync(
        int count,
        int delayMs,
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        for (int i = 0; i < count; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Delay(delayMs, cancellationToken);
            yield return i;
        }
    }
}

// =============================================================================
// OPERATORS
// =============================================================================

/// <summary>
/// A class demonstrating operator overloading.
/// </summary>
public class Money : IEquatable<Money>, IComparable<Money>
{
    /// <summary>
    /// Gets the amount.
    /// </summary>
    public decimal Amount { get; }

    /// <summary>
    /// Gets the currency code.
    /// </summary>
    public string Currency { get; }

    /// <summary>
    /// Creates a new Money instance.
    /// </summary>
    /// <param name="amount">The monetary amount.</param>
    /// <param name="currency">The currency code (e.g., "USD").</param>
    public Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    /// <summary>
    /// Adds two Money values.
    /// </summary>
    /// <param name="a">The first value.</param>
    /// <param name="b">The second value.</param>
    /// <returns>A new Money instance with the sum.</returns>
    /// <exception cref="InvalidOperationException">Thrown when currencies don't match.</exception>
    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException("Cannot add different currencies");
        return new Money(a.Amount + b.Amount, a.Currency);
    }

    /// <summary>
    /// Subtracts two Money values.
    /// </summary>
    /// <param name="a">The value to subtract from.</param>
    /// <param name="b">The value to subtract.</param>
    /// <returns>A new Money instance with the difference.</returns>
    public static Money operator -(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException("Cannot subtract different currencies");
        return new Money(a.Amount - b.Amount, a.Currency);
    }

    /// <summary>
    /// Multiplies money by a scalar.
    /// </summary>
    /// <param name="money">The money value.</param>
    /// <param name="multiplier">The multiplier.</param>
    /// <returns>A new Money instance with the product.</returns>
    public static Money operator *(Money money, decimal multiplier)
        => new(money.Amount * multiplier, money.Currency);

    /// <summary>
    /// Checks equality between two Money values.
    /// </summary>
    /// <param name="a">The first value.</param>
    /// <param name="b">The second value.</param>
    /// <returns><c>true</c> if equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(Money? a, Money? b)
        => a?.Amount == b?.Amount && a?.Currency == b?.Currency;

    /// <summary>
    /// Checks inequality between two Money values.
    /// </summary>
    /// <param name="a">The first value.</param>
    /// <param name="b">The second value.</param>
    /// <returns><c>true</c> if not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(Money? a, Money? b) => !(a == b);

    /// <summary>
    /// Less than comparison.
    /// </summary>
    public static bool operator <(Money a, Money b) => a.CompareTo(b) < 0;

    /// <summary>
    /// Greater than comparison.
    /// </summary>
    public static bool operator >(Money a, Money b) => a.CompareTo(b) > 0;

    /// <summary>
    /// Less than or equal comparison.
    /// </summary>
    public static bool operator <=(Money a, Money b) => a.CompareTo(b) <= 0;

    /// <summary>
    /// Greater than or equal comparison.
    /// </summary>
    public static bool operator >=(Money a, Money b) => a.CompareTo(b) >= 0;

    /// <summary>
    /// Implicit conversion from decimal to USD Money.
    /// </summary>
    /// <param name="amount">The amount in USD.</param>
    public static implicit operator Money(decimal amount) => new(amount, "USD");

    /// <summary>
    /// Explicit conversion from Money to decimal.
    /// </summary>
    /// <param name="money">The money value.</param>
    public static explicit operator decimal(Money money) => money.Amount;

    /// <inheritdoc/>
    public bool Equals(Money? other) => this == other;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Money m && Equals(m);

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Amount, Currency);

    /// <inheritdoc/>
    public int CompareTo(Money? other)
    {
        if (other is null) return 1;
        if (Currency != other.Currency)
            throw new InvalidOperationException("Cannot compare different currencies");
        return Amount.CompareTo(other.Amount);
    }
}

// =============================================================================
// INDEXERS
// =============================================================================

/// <summary>
/// A class demonstrating indexer syntax.
/// </summary>
/// <typeparam name="T">The type of items in the collection.</typeparam>
public class IndexedCollection<T>
{
    private readonly List<T> _items = new();
    private readonly Dictionary<string, T> _namedItems = new();

    /// <summary>
    /// Gets or sets an item by numeric index.
    /// </summary>
    /// <param name="index">The zero-based index.</param>
    /// <returns>The item at the specified index.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when index is out of range.</exception>
    public T this[int index]
    {
        get => _items[index];
        set => _items[index] = value;
    }

    /// <summary>
    /// Gets or sets an item by name.
    /// </summary>
    /// <param name="name">The name of the item.</param>
    /// <returns>The item with the specified name.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when name is not found (get only).</exception>
    public T this[string name]
    {
        get => _namedItems[name];
        set => _namedItems[name] = value;
    }

    /// <summary>
    /// Gets an item by index with a default fallback.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <param name="defaultValue">The default value if index is out of range.</param>
    /// <returns>The item at index or the default value.</returns>
    public T this[int index, T defaultValue]
        => index >= 0 && index < _items.Count ? _items[index] : defaultValue;

    /// <summary>
    /// Adds an item to the collection.
    /// </summary>
    /// <param name="item">The item to add.</param>
    public void Add(T item) => _items.Add(item);

    /// <summary>
    /// Gets the count of items.
    /// </summary>
    public int Count => _items.Count;
}

// =============================================================================
// NESTED TYPES
// =============================================================================

/// <summary>
/// A class containing nested types.
/// </summary>
public class OuterClass
{
    /// <summary>
    /// A nested public class.
    /// </summary>
    public class NestedPublicClass
    {
        /// <summary>
        /// Gets a value from the nested class.
        /// </summary>
        public string Value => "Nested";
    }

    /// <summary>
    /// A nested struct.
    /// </summary>
    public struct NestedStruct
    {
        /// <summary>
        /// The X value.
        /// </summary>
        public int X;

        /// <summary>
        /// The Y value.
        /// </summary>
        public int Y;
    }

    /// <summary>
    /// A nested enum.
    /// </summary>
    public enum NestedEnum
    {
        /// <summary>The first option.</summary>
        First,
        /// <summary>The second option.</summary>
        Second
    }

    /// <summary>
    /// A nested interface.
    /// </summary>
    public interface INestedInterface
    {
        /// <summary>
        /// Does something nested.
        /// </summary>
        void DoSomething();
    }
}

// =============================================================================
// EXTENSION METHODS
// =============================================================================

/// <summary>
/// Extension methods for string manipulation.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Truncates a string to the specified maximum length.
    /// </summary>
    /// <param name="value">The string to truncate.</param>
    /// <param name="maxLength">The maximum length.</param>
    /// <param name="suffix">The suffix to append if truncated. Defaults to "...".</param>
    /// <returns>The truncated string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="maxLength"/> is negative.</exception>
    public static string Truncate(this string value, int maxLength, string suffix = "...")
    {
        ArgumentNullException.ThrowIfNull(value);
        if (maxLength < 0)
            throw new ArgumentOutOfRangeException(nameof(maxLength));

        if (value.Length <= maxLength)
            return value;

        return value[..(maxLength - suffix.Length)] + suffix;
    }

    /// <summary>
    /// Converts a string to title case.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The string in title case.</returns>
    public static string ToTitleCase(this string value)
        => System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
}

/// <summary>
/// Extension methods for <see cref="IEnumerable{T}"/>.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Determines if the enumerable is null or empty.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="source">The source enumerable.</param>
    /// <returns><c>true</c> if null or empty; otherwise, <c>false</c>.</returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? source)
        => source is null || !source.Any();

    /// <summary>
    /// Batches the enumerable into chunks of the specified size.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="source">The source enumerable.</param>
    /// <param name="batchSize">The size of each batch.</param>
    /// <returns>An enumerable of batches.</returns>
    public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
    {
        var batch = new List<T>(batchSize);
        foreach (var item in source)
        {
            batch.Add(item);
            if (batch.Count == batchSize)
            {
                yield return batch;
                batch = new List<T>(batchSize);
            }
        }
        if (batch.Count > 0)
            yield return batch;
    }
}

// =============================================================================
// CONSTRUCTORS
// =============================================================================

/// <summary>
/// Demonstrates various constructor patterns.
/// </summary>
public class ConstructorExamples
{
    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the value.
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Gets the timestamp.
    /// </summary>
    public DateTime Timestamp { get; }

    /// <summary>
    /// Initializes a new instance with default values.
    /// </summary>
    public ConstructorExamples() : this("Default", 0) { }

    /// <summary>
    /// Initializes a new instance with the specified name.
    /// </summary>
    /// <param name="name">The name.</param>
    public ConstructorExamples(string name) : this(name, 0) { }

    /// <summary>
    /// Initializes a new instance with the specified name and value.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="value">The value.</param>
    public ConstructorExamples(string name, int value)
    {
        Name = name;
        Value = value;
        Timestamp = DateTime.UtcNow;
    }

    /// <summary>
    /// A static factory method as an alternative to constructors.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="value">The value.</param>
    /// <returns>A new instance.</returns>
    public static ConstructorExamples Create(string name, int value) => new(name, value);
}

/// <summary>
/// Demonstrates primary constructor syntax (C# 12+).
/// </summary>
/// <param name="name">The name of the configuration.</param>
/// <param name="value">The configuration value.</param>
public class PrimaryConstructorExample(string name, int value)
{
    /// <summary>
    /// Gets the configuration name.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Gets the configuration value.
    /// </summary>
    public int Value { get; } = value;

    /// <summary>
    /// Gets a formatted display string.
    /// </summary>
    public string Display => $"{Name}: {Value}";
}
