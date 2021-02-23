/**
 * TopoSort.cs
 *
 * Implements topological sorting (ordering) with arbitrary data.
 *
 * Licensed under the MIT License:
 *
 * Copyright (c) 2021 Cemalettin Dervis
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 *
 */

using System.Collections.Generic;

namespace TopoSort
{
  /// <summary>
  /// Represents a graph that should be sorted.
  /// </summary>
  /// <typeparam name="T">The type of data a node inside the graph should store.</typeparam>
  public sealed class Graph<T>
  {
    private List<Node> m_Nodes = new List<Node>();

    /// <summary>
    /// Represents the result of a sorting operation.
    /// </summary>
    public sealed class Result
    {
      /// <summary>
      /// Gets a value indicating whether the sorting operation was successful.
      /// </summary>
      public bool IsSuccess => SortedNodes.Count > 0;

      /// <summary>
      /// Gets the sorted list of nodes.
      /// </summary>
      public List<Node> SortedNodes { get; } = new List<Node>();

      /// <summary>
      /// If the graph contains a cyclic dependency, this is the first node in question.
      /// </summary>
      public Node CyclicA;

      /// <summary>
      /// If the graph contains a cyclic dependency, this is the second node in question.
      /// </summary>
      public Node CyclicB;
    }

    /// <summary>
    /// Represents a node inside a graph.
    /// A node stores arbitrary data and a list of its dependencies.
    /// </summary>
    public sealed class Node
    {
      internal List<Node> m_Children = new List<Node>();

      public Node(T data)
      {
        Data = data;
      }

      /// <summary>
      /// Declares that this node depends on another node.
      /// </summary>
      /// <param name="node">The node that this node depends on.</param>
      public void DependsOn(Node node) => m_Children.Add(node);

      /// <summary>
      /// Gets the node's stored data.
      /// </summary>
      public T Data { get; }
    }

    /// <summary>
    /// Clears the graph of all created nodes.
    /// </summary>
    public void Clear() => m_Nodes.Clear();

    /// <summary>
    /// Creates a node that stores a specific value and does not have any
    /// dependencies. Dependencies can be declared using the <see cref="Node.DependsOn(Node)"/>
    /// method on the returned node.
    /// </summary>
    /// <param name="value">The value that should be stored by the node.</param>
    /// <returns>The created node.</returns>
    public Node Create(T value)
    {
      m_Nodes.Add(new Node(value));
      return m_Nodes[^1];
    }

    /// <summary>
    /// Sorts the graph and returns the results.
    /// </summary>
    /// <returns>The results.</returns>
    public Result Sort()
    {
      var result = new Result();

      if (m_Nodes.Count > 0)
      {
        var dead = new List<Node>(m_Nodes.Count);
        var pending = new List<Node>(m_Nodes.Count);

        if (!Visit(m_Nodes, dead, pending, result))
          result.SortedNodes.Clear();

        Clear();
      }

      return result;
    }

    private bool Visit(IEnumerable<Node> graph, List<Node> dead, List<Node> pending, Result res)
    {
      foreach (var n in graph)
      {
        if (dead.Contains(n))
          continue;

        if (pending.Contains(n))
        {
          res.CyclicA = n;
          res.CyclicB = pending[^1];
          return false;
        }
        else
          pending.Add(n);

        if (!Visit(n.m_Children, dead, pending, res))
          return false;

        pending.Remove(n);
        dead.Add(n);
        res.SortedNodes.Add(n);
      }

      return true;
    }
  }
}
