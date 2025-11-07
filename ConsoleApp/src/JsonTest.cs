using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleApp;

// ---------- Интерфейсы и классы ----------
public interface IShape
{
    string Type { get; }
}

public class Circle : IShape
{
    public string Type => "circle";
    public double Radius { get; set; }
}

public class Rectangle : IShape
{
    public string Type => "rectangle";
    public double Width { get; set; }
    public double Height { get; set; }
}

// ---------- Кастомный конвертер ----------
public class ShapeConverter : JsonConverter<IShape>
{
    public override IShape Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        var type = root.GetProperty("Type").GetString();

        IShape? result = type switch
        {
            "circle" => JsonSerializer.Deserialize(root.GetRawText(), MyJsonContext.Default.Circle),
            "rectangle" => JsonSerializer.Deserialize(root.GetRawText(), MyJsonContext.Default.Rectangle),
            _ => throw new JsonException($"Unknown shape type: {type}")
        };
        if(result is null)
            throw new JsonException($"Unknown shape type: {type}");
        return result;
    }

    public override void Write(Utf8JsonWriter writer, IShape value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case Circle c:
                JsonSerializer.Serialize(writer, c, MyJsonContext.Default.Circle);
                break;
            case Rectangle r:
                JsonSerializer.Serialize(writer, r, MyJsonContext.Default.Rectangle);
                break;
            default:
                throw new JsonException($"Unsupported shape type: {value.GetType().Name}");
        }
    }
}

// ---------- Пример использования ----------
public class JsonTest
{
    public static void Test() {
        var shapes = new List<IShape> {
            new Circle { Radius = 5.2 },
            new Rectangle { Width = 3.0, Height = 4.0 }
        };

        // var options = new JsonSerializerOptions {
        //     WriteIndented = true,
        //     Converters = { new ShapeConverter() },
        //     TypeInfoResolver = MyJsonContext.Default
        // };

        // Сериализация
        var json = JsonSerializer.Serialize(shapes, MyJsonContext.Default.ListIShape);
        Console.WriteLine("JSON:");
        Console.WriteLine(json);

        // Десериализация
        var restored = JsonSerializer.Deserialize(json, MyJsonContext.Default.ListIShape);
        Console.WriteLine("\nRestored:");
        if (restored is not null)
            foreach (var shape in restored) {
                Console.WriteLine($"{shape.Type}");
            }
    }
}

// ---------- Контекст для AOT ----------
[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(List<IShape>))]
[JsonSerializable(typeof(Circle))]
[JsonSerializable(typeof(Rectangle))]
internal partial class MyJsonContext : JsonSerializerContext { }