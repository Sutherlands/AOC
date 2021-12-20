using DotNetty.Common.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode
{
  public static class Day18
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay18.txt").ToList();

      Node existingNode = null;
      foreach(var line in lines)
      {
        var index = 0;
        var newNode = SingleNodeParser.ParseNode(line, ref index);
        existingNode = existingNode == null ? newNode : AddNodes(existingNode, newNode);
      }
      Console.WriteLine(existingNode.GetMagnitude());
     }

    public static void RunPart2()
    {
      var sw = Stopwatch.StartNew();
      var lines = File.ReadAllLines("./PuzzleInputDay18.txt").ToList();
      var nodes = lines.Select(l => { var index = 0; return SingleNodeParser.ParseNode(l, ref index); });

      var magnitudes = lines.SelectMany(lhs => lines.Select(rhs =>
      {
        if (lhs == rhs)
        {
          return 0;
        }
        var lhsNode = SingleNodeParser.ParseNode(lhs);
        var rhsNode = SingleNodeParser.ParseNode(rhs);
        var magnitude = AddNodes(lhsNode, rhsNode).GetMagnitude();
        return magnitude;
      }));

      Console.WriteLine(sw.ElapsedMilliseconds);

      Console.WriteLine(magnitudes.Max());
    }

    public static GraphNode AddNodes(Node lhs, Node rhs)
    {
      var outerNode = new GraphNode { Left = lhs, Right = rhs };
      while (Reduce(outerNode)) { }
      return outerNode;
    }

    public static bool Reduce(GraphNode node)
    {
      var deepNode = node.GetDeepNode(4);
      if (deepNode != null)
      {
        var parent = deepNode.Parent;

        var leftLiteral = parent.FindNextLiteralNodeUp(deepNode, Direction.Left);
        var rightLiteral = parent.FindNextLiteralNodeUp(deepNode, Direction.Right);

        if (parent.Left == deepNode)
        {
          parent.Left = new LiteralNode { Value = 0 };
        }
        else
        {
          parent.Right = new LiteralNode { Value = 0 };
        }

        if(leftLiteral != null)
        {
          leftLiteral.Value += ((LiteralNode)deepNode.Left).Value;
        }

        if (rightLiteral != null)
        {
          rightLiteral.Value += ((LiteralNode)deepNode.Right).Value;
        }

        return true;
      }

      var largeLiteral = node.GetLiteralNodes().FirstOrDefault(n => n.Value >= 10);
      if(largeLiteral != null)
      {
        var parent = largeLiteral.Parent;
        var replacedNode = new GraphNode();
        replacedNode.Left = new LiteralNode { Value = largeLiteral.Value / 2 };
        replacedNode.Right = new LiteralNode { Value = (largeLiteral.Value + 1) / 2 };

        if(parent.Left == largeLiteral)
        {
          parent.Left = replacedNode;
        }
        else
        {
          parent.Right = replacedNode;
        }

        return true;
      }

      return false;
    }

    public enum Direction
    {
      Right,
      Left
    }

    public static class SingleNodeParser
    {
      public static Node ParseNode(string s)
      {
        var index = 0;
        return ParseNode(s, ref index);
      }

      public static Node ParseNode(string s, ref int index)
      {
        if (char.IsDigit(s[index]))
        {
          var value = s[index] - '0';
          index++;
          return new LiteralNode { Value = value };
        }

        if (s[index] != '[')
        {
          throw new InvalidDataException();
        }
        index++;

        var leftNode = ParseNode(s, ref index);

        if (s[index] != ',')
        {
          throw new InvalidDataException();
        }
        index++;

        var rightNode = ParseNode(s, ref index);

        if (s[index] != ']')
        {
          throw new InvalidDataException();
        }
        index++;

        var containingNode = new GraphNode { Left = leftNode, Right = rightNode };
        return containingNode;
      }
    }

    public abstract class Node
    {
      public GraphNode Parent { get; set; }
      public abstract GraphNode GetDeepNode(int remainingDepth);
      public abstract LiteralNode FindNextLiteralNodeUp(Node previousNode, Direction direction);
      public abstract LiteralNode FindNextLiteralNodeDown(Direction direction);
      public abstract IEnumerable<LiteralNode> GetLiteralNodes();
      public abstract int GetMagnitude();


    }

    public class LiteralNode : Node
    {
      public int Value { get; set; }

      public override LiteralNode FindNextLiteralNodeDown(Direction direction)
      {
        return this;
      }

      public override LiteralNode FindNextLiteralNodeUp(Node previousNode, Direction direction)
      {
        return this;
      }

      public override GraphNode GetDeepNode(int remainingDepth)
      {
        return null;
      }

      public override IEnumerable<LiteralNode> GetLiteralNodes()
      {
        yield return this;
      }

      public override int GetMagnitude()
      {
        return Value;
      }

      public override string ToString()
      {
        return Value.ToString();
      }
    }

    public class GraphNode : Node
    {
      private Node _left;
      private Node _right;

      public Node Left
      {
        get => _left; set
        {
          _left = value;
          _left.Parent = this;
        }
      }

      public Node Right
      {
        get => _right; set
        {
          _right = value;
          _right.Parent = this;
        }
      }

      public override LiteralNode FindNextLiteralNodeDown(Direction direction)
      {
        var targetNode = direction == Direction.Left ? Right : Left;
        return targetNode.FindNextLiteralNodeDown(direction);
      }

      public override LiteralNode FindNextLiteralNodeUp(Node previousNode, Direction direction)
      {
        var closerNode = direction == Direction.Left ? Right : Left;
        var furtherNode = direction == Direction.Left ? Left : Right;
        if (closerNode == previousNode)
        {
          return furtherNode.FindNextLiteralNodeDown(direction);
        }

        if (Parent != null)
        {
          return Parent.FindNextLiteralNodeUp(this, direction);
        }

        return null;
      }

      public override GraphNode GetDeepNode(int remainingDepth)
      {
        if (remainingDepth == 0)
        {
          return this;
        }

        var left = Left.GetDeepNode(remainingDepth - 1);
        if (left != null)
        {
          return left;
        }
        return Right.GetDeepNode(remainingDepth - 1);
      }

      public override IEnumerable<LiteralNode> GetLiteralNodes()
      {
        foreach (var node in Left.GetLiteralNodes())
        {
          yield return node;
        }

        foreach (var node in Right.GetLiteralNodes())
        {
          yield return node;
        }
      }

      public override int GetMagnitude()
      {
        return Left.GetMagnitude() * 3 + Right.GetMagnitude() * 2;
      }

      public override string ToString()
      {
        return $"[{Left},{Right}]";
      }
    }
  }
}
