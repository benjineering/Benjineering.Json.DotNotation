## Benjineering.Json.DotNotation

## Helpers and extension methods for querying System.Text.Json.JsonElement objects via dot notation.

### Usage

```C#
using Benjineering.Json.DotNotation;
```

### Basic nested properties

```C#
// this will throw a KeyNotFoundException if user is null or undefined
var result = JsonElementHelpers.GetPropertyAtPath(jsonElement, "user.email");

// the same methods are also available as extension methods on System.Text.Json.JsonElement
var result = jsonElement.GetPropertyAtPath("user.address.country");
```

### Optional chaining (aka the JS question mark operator)

```C#
// this will return a JsonElement with a ValueKind of JsonValueKind.Undefined if user is null or undefined
var result = jsonElement.GetPropertyAtPath("user?.id");
```

### Bonus convenience method

```C#
jsonElement.IsNullOrUndefined();
```
