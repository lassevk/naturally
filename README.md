# Naturally

![.NET Core](https://github.com/lassevk/naturally/workflows/.NET%20Core/badge.svg)

### Natural sorting order library for .NET Standard

Supported frameworks:

* .NET Standard 1.6
* .NET Standard 2.0
* .NET Standard 2.1

See the [.NET Standard page](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) for more information
about which .NET Framework and .NET Core versions these versions applies to.

### How to use it

First install the package into your project, either using your IDE or other tooling, or using the following command:

    Install-Package lvk.naturally

Then, to use it, anywhere you can specify a `StringComparer` instance you can now use one of the following:

* NaturalSortOrderStringComparer.CurrentCulture
* NaturalSortOrderStringComparer.CurrentCultureIgnoreCase
* NaturalSortOrderStringComparer.InvariantCulture
* NaturalSortOrderStringComparer.InvariantCultureIgnoreCase
* NaturalSortOrderStringComparer.Ordinal
* NaturalSortOrderStringComparer.OrdinalIgnoreCase

All of these correspond to the existing `StringComparer.*`, only with "natural sort order" rules applied.

*(Note that the two InvariantCulture options above does not exist for .NET Standard 1.6 due to the corresponding options only being added to .NET Standard 2.0)*

### What is natural sort order

*Natural sort order* treats numbers as numbes and not as digits forming a text.

This impacts sorting when text contains numbers, such as the following items:

    file10.txt
    file2.txt
    file1.txt
    file11.txt

With "normal" sort order, using lexicographical ordering, these will be treated strictly as text, and thus `1<2` as *characters* will apply, resulting in the following sort order:

    file1.txt
    file10.txt
    file11.txt
    file2.txt

Even though 2 is less than 10 or 11, 2 as a character comes after 1 as a character.

With natural sort order applied, the order becomes more "natural" in terms of what we expect:

    file1.txt
    file2.txt
    file10.txt
    file11.txt
