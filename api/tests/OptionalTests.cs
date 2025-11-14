using System.Text.Json;
using System.Text.Json.Serialization;
using Koworking.Api.Infrastructure;

namespace Api.Tests;

public class OptionalTests
{
    private static readonly JsonSerializerOptions JsonOpts = JsonDefaults.Options;
    public record TestClass
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<string> Prop { get; set; }
    }
    
    [Fact]
    public void TestSerialization()
    {
        Assert.Empty(JsonSerializer.SerializeToNode(new TestClass(), JsonOpts)!.AsObject());
        Assert.Single(JsonSerializer.SerializeToNode(new TestClass
        {
            Prop = "test"
        }, JsonOpts)!.AsObject());
    }

    [Fact]
    public void TestDeserialization()
    {
        Assert.False(JsonSerializer.Deserialize<TestClass>("{}", JsonOpts)!.Prop.HasValue);
        Assert.True(JsonSerializer.Deserialize<TestClass>(json:
            """
            { "prop": "test" }
            """, JsonOpts)!.Prop.HasValue);
    }
}