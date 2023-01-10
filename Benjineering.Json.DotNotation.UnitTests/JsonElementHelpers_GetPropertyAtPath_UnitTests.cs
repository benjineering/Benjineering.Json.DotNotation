using System.Text.Json;

namespace Benjineering.Json.DotNotation.UnitTests
{
    public class JsonElementHelpers_GetPropertyAtPath_UnitTests
    {
        #region dots

        [Fact]
        public void OneDot()
        {
            var json = JsonSerializer.Deserialize<JsonElement>(@"{ ""a"": { ""b"": ""c"" } }");
            var result = JsonElementHelpers.GetPropertyAtPath(json, "a.b");
            Assert.Equal("c", result.GetString());
        }

        [Fact]
        public void TwoDots()
        {
            var json = JsonSerializer.Deserialize<JsonElement>(@"{ ""a"": { ""b"": { ""c"": ""d"" } } }");
            var result = JsonElementHelpers.GetPropertyAtPath(json, "a.b.c");
            Assert.Equal("d", result.GetString());
        }

        [Fact]
        public void ThreeDots()
        {
            var json = JsonSerializer.Deserialize<JsonElement>(@"{ ""a"": { ""b"": { ""c"": { ""d"": ""e"" } } } }");
            var result = JsonElementHelpers.GetPropertyAtPath(json, "a.b.c.d");
            Assert.Equal("e", result.GetString());
        }

        #endregion
        #region question marks

        [Fact]
        public void QuestionMark_UndefinedProperty()
        {
            var json = JsonSerializer.Deserialize<JsonElement>("{}");
            var result = JsonElementHelpers.GetPropertyAtPath(json, "a?.b");
            Assert.Equal(JsonValueKind.Undefined, result.ValueKind);
        }

        [Fact]
        public void QuestionMark_NullProperty()
        {
            var json = JsonSerializer.Deserialize<JsonElement>(@"{ ""a"": null }");
            var result = JsonElementHelpers.GetPropertyAtPath(json, "a?.b");
            Assert.Equal(JsonValueKind.Undefined, result.ValueKind);
        }

        [Fact]
        public void QuestionMark_NotNullProperty()
        {
            var json = JsonSerializer.Deserialize<JsonElement>(@"{ ""a"": { ""b"": ""c"" } }");
            var result = JsonElementHelpers.GetPropertyAtPath(json, "a?.b");
            Assert.Equal("c", result.GetString());
        }

        [Fact]
        public void EarlyQuestionMark_NullProperty()
        {
            var json = JsonSerializer.Deserialize<JsonElement>(@"{ ""a"": null }");
            var result = JsonElementHelpers.GetPropertyAtPath(json, "a?.b.c.d");
            Assert.Equal(JsonValueKind.Undefined, result.ValueKind);
        }

        [Fact]
        public void EarlyQuestionMark_UndefinedProperty()
        {
            var json = JsonSerializer.Deserialize<JsonElement>("{}");
            var result = JsonElementHelpers.GetPropertyAtPath(json, "a?.b.c.d");
            Assert.Equal(JsonValueKind.Undefined, result.ValueKind);
        }

        [Fact]
        public void LateQuestionMark_NotNullProperty()
        {
            var json = JsonSerializer.Deserialize<JsonElement>("{ \"a\": { \"b\": { \"c\": { \"d\": \"e\" } } } }");
            var result = JsonElementHelpers.GetPropertyAtPath(json, "a.b.c?.d");
            Assert.Equal("e", result.GetString());
        }

        #endregion
        #region arrays

        [Fact]
        public void ArrayIndex_InRange()
        {
            var json = JsonSerializer.Deserialize<JsonElement>(@"{ ""a"": { ""b"": [ 92, 65 ] } }");
            var result = JsonElementHelpers.GetPropertyAtPath(json, "a?.b.1");
            Assert.Equal(65, result.GetInt32());
        }

        [Fact]
        public void ArrayIndex_OutOfRange()
        {
            var json = JsonSerializer.Deserialize<JsonElement>(@"{ ""a"": { ""b"": [ 92, 65 ] } }");
            var result = JsonElementHelpers.GetPropertyAtPath(json, "a?.b.2");
            Assert.Equal(JsonValueKind.Undefined, result.ValueKind);
        }

        #endregion
        #region bad syntax

        [Fact]
        public void TwoConsecutiveDots()
        {
            var json = JsonSerializer.Deserialize<JsonElement>("{}");
            Assert.Throws<KeyNotFoundException>(() => JsonElementHelpers.GetPropertyAtPath(json, "a..b"));
        }

        [Fact]
        public void QuestionMarkAfterDot()
        {
            var json = JsonSerializer.Deserialize<JsonElement>("{}");
            Assert.Throws<KeyNotFoundException>(() => JsonElementHelpers.GetPropertyAtPath(json, "a.?b"));
        }

        #endregion
        #region bad paths

        [Fact]
        public void NullRef()
        {
            var json = JsonSerializer.Deserialize<JsonElement>(@"{ ""a"": null }");
            Assert.Throws<KeyNotFoundException>(() => JsonElementHelpers.GetPropertyAtPath(json, "a.b"));
        }

        [Fact]
        public void UndefinedRef()
        {
            var json = JsonSerializer.Deserialize<JsonElement>("{}");
            Assert.Throws<KeyNotFoundException>(() => JsonElementHelpers.GetPropertyAtPath(json, "a.b"));
        }

        #endregion
    }
}
