// =============================================================================
// Example.XmlDocSamples - Comprehensive XML Documentation Examples
// =============================================================================
// This project demonstrates all C# syntax elements that support XML documentation.
// It serves as both a reference and a test fixture for MDGen.
// =============================================================================

namespace Example.XmlDocSamples;

// =============================================================================
// CLASSES
// =============================================================================

/// <summary>
/// A basic class demonstrating fundamental XML documentation.
/// </summary>
/// <remarks>
/// This class shows how to document a simple class with properties and methods.
/// Use <see cref="BasicClass"/> when you need a simple example.
/// </remarks>
/// <example>
/// <code>
/// var instance = new BasicClass();
/// instance.Name = "Example";
/// var result = instance.GetGreeting();
/// </code>
/// </example>
public class BasicClass
{
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name of the instance.</value>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets a greeting message.
    /// </summary>
    /// <returns>A greeting string including the <see cref="Name"/>.</returns>
    public string GetGreeting() => $"Hello, {Name}!";
}

/// <summary>
/// A static class containing utility methods.
/// </summary>
/// <remarks>
/// Static classes cannot be instantiated and contain only static members.
/// </remarks>
public static class StaticUtilityClass
{
    /// <summary>
    /// Adds two integers.
    /// </summary>
    /// <param name="a">The first integer.</param>
    /// <param name="b">The second integer.</param>
    /// <returns>The sum of <paramref name="a"/> and <paramref name="b"/>.</returns>
    public static int Add(int a, int b) => a + b;
}

/// <summary>
/// A sealed class that cannot be inherited.
/// </summary>
public sealed class SealedClass
{
    /// <summary>
    /// Gets the sealed value.
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="SealedClass"/>.
    /// </summary>
    /// <param name="value">The initial value.</param>
    public SealedClass(int value) => Value = value;
}

/// <summary>
/// An abstract base class for shapes.
/// </summary>
/// <remarks>
/// Inherit from this class to create specific shape implementations.
/// </remarks>
public abstract class AbstractShape
{
    /// <summary>
    /// Gets the name of the shape.
    /// </summary>
    public abstract string ShapeName { get; }

    /// <summary>
    /// Calculates the area of the shape.
    /// </summary>
    /// <returns>The area as a double.</returns>
    public abstract double CalculateArea();

    /// <summary>
    /// Gets a description of the shape.
    /// </summary>
    /// <returns>A formatted description string.</returns>
    public virtual string GetDescription() => $"This is a {ShapeName}";
}

/// <summary>
/// A circle shape implementation.
/// </summary>
/// <remarks>
/// Inherits from <see cref="AbstractShape"/> and implements circle-specific calculations.
/// </remarks>
public class Circle : AbstractShape
{
    /// <summary>
    /// Gets the radius of the circle.
    /// </summary>
    public double Radius { get; }

    /// <summary>
    /// Initializes a new circle with the specified radius.
    /// </summary>
    /// <param name="radius">The radius of the circle. Must be positive.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="radius"/> is less than or equal to zero.</exception>
    public Circle(double radius)
    {
        if (radius <= 0)
            throw new ArgumentOutOfRangeException(nameof(radius), "Radius must be positive.");
        Radius = radius;
    }

    /// <inheritdoc/>
    public override string ShapeName => "Circle";

    /// <inheritdoc/>
    public override double CalculateArea() => Math.PI * Radius * Radius;
}

// =============================================================================
// GENERIC CLASSES
// =============================================================================

/// <summary>
/// A generic container class.
/// </summary>
/// <typeparam name="T">The type of item to contain.</typeparam>
public class GenericContainer<T>
{
    /// <summary>
    /// Gets or sets the contained item.
    /// </summary>
    public T? Item { get; set; }

    /// <summary>
    /// Determines if the container has an item.
    /// </summary>
    /// <returns><c>true</c> if <see cref="Item"/> is not null; otherwise, <c>false</c>.</returns>
    public bool HasItem() => Item is not null;
}

/// <summary>
/// A generic class with multiple type parameters and constraints.
/// </summary>
/// <typeparam name="TKey">The key type, must be non-null.</typeparam>
/// <typeparam name="TValue">The value type, must be a class with a parameterless constructor.</typeparam>
public class GenericDictionary<TKey, TValue>
    where TKey : notnull
    where TValue : class, new()
{
    private readonly Dictionary<TKey, TValue> _items = new();

    /// <summary>
    /// Adds or updates an item in the dictionary.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public void AddOrUpdate(TKey key, TValue value) => _items[key] = value;

    /// <summary>
    /// Gets a value by key, or creates a new instance if not found.
    /// </summary>
    /// <param name="key">The key to look up.</param>
    /// <returns>The existing value or a new instance of <typeparamref name="TValue"/>.</returns>
    public TValue GetOrCreate(TKey key)
    {
        if (!_items.TryGetValue(key, out var value))
        {
            value = new TValue();
            _items[key] = value;
        }
        return value;
    }
}

// =============================================================================
// STRUCTS
// =============================================================================

/// <summary>
/// A simple value type representing a 2D point.
/// </summary>
/// <remarks>
/// Structs are value types and are copied when assigned.
/// </remarks>
public struct Point2D
{
    /// <summary>
    /// Gets the X coordinate.
    /// </summary>
    public double X { get; }

    /// <summary>
    /// Gets the Y coordinate.
    /// </summary>
    public double Y { get; }

    /// <summary>
    /// Initializes a new point at the specified coordinates.
    /// </summary>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    public Point2D(double x, double y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Calculates the distance from the origin.
    /// </summary>
    /// <returns>The Euclidean distance from (0, 0).</returns>
    public double DistanceFromOrigin() => Math.Sqrt(X * X + Y * Y);
}

/// <summary>
/// A readonly struct for immutable data.
/// </summary>
/// <remarks>
/// Readonly structs guarantee that all members are readonly,
/// enabling compiler optimizations.
/// </remarks>
public readonly struct ImmutableVector
{
    /// <summary>
    /// Gets the X component.
    /// </summary>
    public double X { get; }

    /// <summary>
    /// Gets the Y component.
    /// </summary>
    public double Y { get; }

    /// <summary>
    /// Gets the Z component.
    /// </summary>
    public double Z { get; }

    /// <summary>
    /// Creates a new immutable vector.
    /// </summary>
    /// <param name="x">The X component.</param>
    /// <param name="y">The Y component.</param>
    /// <param name="z">The Z component.</param>
    public ImmutableVector(double x, double y, double z) => (X, Y, Z) = (x, y, z);

    /// <summary>
    /// Calculates the magnitude of the vector.
    /// </summary>
    /// <returns>The magnitude as a double.</returns>
    public double Magnitude() => Math.Sqrt(X * X + Y * Y + Z * Z);
}

/// <summary>
/// A ref struct that can only exist on the stack.
/// </summary>
/// <remarks>
/// Ref structs cannot be boxed or used in async methods.
/// They are useful for high-performance scenarios with <see cref="Span{T}"/>.
/// </remarks>
public ref struct RefStackOnly
{
    /// <summary>
    /// The underlying span of integers.
    /// </summary>
    public Span<int> Data;

    /// <summary>
    /// Creates a new ref struct wrapping the given span.
    /// </summary>
    /// <param name="data">The span to wrap.</param>
    public RefStackOnly(Span<int> data) => Data = data;

    /// <summary>
    /// Gets the sum of all elements.
    /// </summary>
    /// <returns>The sum of elements in <see cref="Data"/>.</returns>
    public int Sum()
    {
        int sum = 0;
        foreach (var item in Data)
            sum += item;
        return sum;
    }
}

// =============================================================================
// RECORDS
// =============================================================================

/// <summary>
/// A simple record for immutable data with value equality.
/// </summary>
/// <param name="FirstName">The person's first name.</param>
/// <param name="LastName">The person's last name.</param>
/// <remarks>
/// Records provide value-based equality and immutability by default.
/// </remarks>
public record Person(string FirstName, string LastName)
{
    /// <summary>
    /// Gets the full name of the person.
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";
}

/// <summary>
/// A record with additional members beyond the primary constructor.
/// </summary>
/// <param name="Id">The unique identifier.</param>
/// <param name="Name">The product name.</param>
/// <param name="Price">The product price.</param>
public record Product(int Id, string Name, decimal Price)
{
    /// <summary>
    /// Gets or sets the quantity in stock.
    /// </summary>
    public int StockQuantity { get; init; }

    /// <summary>
    /// Determines if the product is in stock.
    /// </summary>
    /// <returns><c>true</c> if <see cref="StockQuantity"/> is greater than zero.</returns>
    public bool IsInStock() => StockQuantity > 0;
}

/// <summary>
/// A record struct (value type record).
/// </summary>
/// <param name="X">The X coordinate.</param>
/// <param name="Y">The Y coordinate.</param>
/// <remarks>
/// Record structs combine the value semantics of structs with the
/// convenience features of records.
/// </remarks>
public record struct Coordinate(double X, double Y)
{
    /// <summary>
    /// Creates a coordinate at the origin.
    /// </summary>
    public static Coordinate Origin => new(0, 0);
}

/// <summary>
/// A readonly record struct for maximum immutability.
/// </summary>
/// <param name="Numerator">The numerator of the fraction.</param>
/// <param name="Denominator">The denominator of the fraction.</param>
public readonly record struct Fraction(int Numerator, int Denominator)
{
    /// <summary>
    /// Gets the decimal value of the fraction.
    /// </summary>
    public double Value => (double)Numerator / Denominator;
}

// =============================================================================
// INTERFACES
// =============================================================================

/// <summary>
/// A basic interface defining a repository pattern.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
/// <typeparam name="TKey">The key type.</typeparam>
public interface IRepository<T, TKey>
{
    /// <summary>
    /// Gets an entity by its key.
    /// </summary>
    /// <param name="key">The entity key.</param>
    /// <returns>The entity if found; otherwise, <c>null</c>.</returns>
    T? GetById(TKey key);

    /// <summary>
    /// Gets all entities.
    /// </summary>
    /// <returns>An enumerable of all entities.</returns>
    IEnumerable<T> GetAll();

    /// <summary>
    /// Adds a new entity.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    void Add(T entity);

    /// <summary>
    /// Deletes an entity by its key.
    /// </summary>
    /// <param name="key">The key of the entity to delete.</param>
    /// <returns><c>true</c> if the entity was deleted; otherwise, <c>false</c>.</returns>
    bool Delete(TKey key);
}

/// <summary>
/// An interface with default implementation.
/// </summary>
public interface ILoggable
{
    /// <summary>
    /// Gets the log prefix for this object.
    /// </summary>
    string LogPrefix { get; }

    /// <summary>
    /// Logs a message with the prefix.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <returns>The formatted log message.</returns>
    string Log(string message) => $"[{LogPrefix}] {message}";
}

// =============================================================================
// ENUMS
// =============================================================================

/// <summary>
/// Represents the days of the week.
/// </summary>
public enum DayOfWeek
{
    /// <summary>Sunday - the first day of the week.</summary>
    Sunday = 0,
    /// <summary>Monday - the second day of the week.</summary>
    Monday = 1,
    /// <summary>Tuesday - the third day of the week.</summary>
    Tuesday = 2,
    /// <summary>Wednesday - the fourth day of the week.</summary>
    Wednesday = 3,
    /// <summary>Thursday - the fifth day of the week.</summary>
    Thursday = 4,
    /// <summary>Friday - the sixth day of the week.</summary>
    Friday = 5,
    /// <summary>Saturday - the seventh day of the week.</summary>
    Saturday = 6
}

/// <summary>
/// File access flags that can be combined.
/// </summary>
/// <remarks>
/// This enum uses the <see cref="FlagsAttribute"/> to allow bitwise combinations.
/// </remarks>
[Flags]
public enum FileAccessFlags
{
    /// <summary>No access.</summary>
    None = 0,
    /// <summary>Read access.</summary>
    Read = 1,
    /// <summary>Write access.</summary>
    Write = 2,
    /// <summary>Execute access.</summary>
    Execute = 4,
    /// <summary>Full access (read, write, and execute).</summary>
    Full = Read | Write | Execute
}

// =============================================================================
// DELEGATES
// =============================================================================

/// <summary>
/// A delegate for handling calculation operations.
/// </summary>
/// <param name="a">The first operand.</param>
/// <param name="b">The second operand.</param>
/// <returns>The result of the calculation.</returns>
public delegate int CalculationHandler(int a, int b);

/// <summary>
/// A generic delegate for transforming values.
/// </summary>
/// <typeparam name="TInput">The input type.</typeparam>
/// <typeparam name="TOutput">The output type.</typeparam>
/// <param name="input">The input value.</param>
/// <returns>The transformed output value.</returns>
public delegate TOutput Transformer<TInput, TOutput>(TInput input);

// =============================================================================
// EVENTS
// =============================================================================

/// <summary>
/// Event arguments for data change events.
/// </summary>
/// <typeparam name="T">The type of data that changed.</typeparam>
public class DataChangedEventArgs<T> : EventArgs
{
    /// <summary>
    /// Gets the old value before the change.
    /// </summary>
    public T? OldValue { get; }

    /// <summary>
    /// Gets the new value after the change.
    /// </summary>
    public T? NewValue { get; }

    /// <summary>
    /// Initializes new event arguments.
    /// </summary>
    /// <param name="oldValue">The value before the change.</param>
    /// <param name="newValue">The value after the change.</param>
    public DataChangedEventArgs(T? oldValue, T? newValue)
    {
        OldValue = oldValue;
        NewValue = newValue;
    }
}

/// <summary>
/// A class that raises events when data changes.
/// </summary>
/// <typeparam name="T">The type of data being observed.</typeparam>
public class ObservableValue<T>
{
    private T? _value;

    /// <summary>
    /// Occurs when the value changes.
    /// </summary>
    public event EventHandler<DataChangedEventArgs<T>>? ValueChanged;

    /// <summary>
    /// Gets or sets the value, raising <see cref="ValueChanged"/> on changes.
    /// </summary>
    public T? Value
    {
        get => _value;
        set
        {
            var oldValue = _value;
            _value = value;
            OnValueChanged(new DataChangedEventArgs<T>(oldValue, value));
        }
    }

    /// <summary>
    /// Raises the <see cref="ValueChanged"/> event.
    /// </summary>
    /// <param name="e">The event arguments.</param>
    protected virtual void OnValueChanged(DataChangedEventArgs<T> e)
    {
        ValueChanged?.Invoke(this, e);
    }
}
