using System.Text.Json;

namespace Benjineering.Json.DotNotation.UnitTests
{
    public class JsonElementHelpers_IsNullOrUndefined_UnitTests
    {
        [Fact]
        public void Null()
        {
            var json = JsonSerializer.Deserialize<JsonElement>("null");
            var result = JsonElementHelpers.IsNullOrUndefined(json);
            Assert.True(result);
        }

        [Fact]
        public void Undefined()
        {
            var json = new JsonElement();
            var result = JsonElementHelpers.IsNullOrUndefined(json);
            Assert.True(result);
        }

        [Fact]
        public void HasValue()
        {
            var json = JsonSerializer.Deserialize<JsonElement>("{}");
            var result = JsonElementHelpers.IsNullOrUndefined(json);
            Assert.False(result);
        }
    }
}