using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Koworking.Api.Infrastructure;

/// <summary>
/// Represents a value that may or may not exist.
/// Used for JavaScript undefined value.
/// </summary>
/// <remarks>
/// When using with json serialization, property must be marked with
/// <code>
/// [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
/// </code>
/// </remarks>
[DebuggerDisplay("{ToString(),nq")]
public readonly record struct Optional<T>
{
    private readonly T? _value;
    
    public bool HasValue { get; }
    public T Value => HasValue
        ? _value!
        : throw new InvalidOperationException("Value not set");

    public T? ValueOrDefault => HasValue ? _value : default;

    private Optional(bool hasValue, T? value)
    {
        HasValue = hasValue;
        _value = value;
    }
    
    public T Or(T fallback) => HasValue ? _value! : fallback;

    public static Optional<T> FromValue(T value) => new(true, value);
    public static Optional<T> Empty => new(false, default);

    public bool Equals(Optional<T>? other) =>
        other is { } another && 
        HasValue == another.HasValue &&
        Value?.Equals(other.Value) is true;

    public override int GetHashCode() => ValueOrDefault?.GetHashCode() ?? 0;

    public override string? ToString() => HasValue ? Value?.ToString() : "undefined";
    
    public static implicit operator Optional<T>(T value) => FromValue(value);
}

public class OptionalJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => 
        typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Optional<>);

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options) => 
        Activator.CreateInstance(typeof(Converter<>).MakeGenericType(typeToConvert.GetGenericArguments().Single())) 
            as JsonConverter;
    
    private class Converter<T> : JsonConverter<Optional<T>>
    {
        public override Optional<T> Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options) =>
            Optional<T>.FromValue(JsonSerializer.Deserialize<T>(ref reader, options)!);

        public override void Write(Utf8JsonWriter writer, Optional<T> value, JsonSerializerOptions options)
            => JsonSerializer.Serialize(writer, value.Value, options);
    }
}