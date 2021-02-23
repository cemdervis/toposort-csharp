# toposort C#

An easy to use topological sorting library for C# with support for cyclic dependency checking.

If you're looking for a C++ version of this library, check it out [here](https://github.com/cemdervis/toposort-cpp).

The library consists of a single file (TopoSort.cs), which is intended to be drag-and-dropped into your project.

Usage:

```csharp
var g = new Graph<char>();

var a = g.Create('a');
var b = g.Create('b');
var c = g.Create('c');
var d = g.Create('d');
var e = g.Create('e');

b.DependsOn(a);
b.DependsOn(c);
d.DependsOn(b);
e.DependsOn(c);

var result = g.Sort();

Console.WriteLine(result.IsSuccess
? $"Sorting successful! Order: {string.Join(", ", result.SortedNodes.Select(n => n.Data))}"
: $"Failed to sort! Cyclic dependency between {result.CyclicA.Data} and {result.CyclicB.Data}");
```