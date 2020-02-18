# Naturally

Natural sorting order library for .NET Standard

The library adds a `StringComparer` that implement the following rules:

* Everything is managed C#
* All Unicode characters that have a numerical value is considered when comparing numerical sections
* Whitespace doesn't matter, unless it is the only difference, and then "less" whitespace comes before "more" whitespace
    * Example: `"A 10"` comes before `"A11"`
    * ... but `"A10"` comes before `"A 10"`
* Possibility to use custom string comparison rules for the text, from [`StringComparison` enum](https://docs.microsoft.com/en-us/dotnet/api/system.stringcomparison?view=netframework-4.8)
    with default value `StringComparison.CurrentCultureIgnoreCase`