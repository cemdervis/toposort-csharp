using System;
using System.Collections.Generic;
using System.Linq;
using TopoSort;

namespace TopoSortExample
{
  class Program
  {
    static void Main()
    {
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

      Console.ReadLine();
    }
  }
}
