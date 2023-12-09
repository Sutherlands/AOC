using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace _2023
{
  public static class Day8
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay8.txt").ToList();
      var instructions = lines[0];
      var nodes = lines.Skip(2).Select(l => {
        var p1 = l.Split(" = (");
        var p2 = p1[1].Split(", ");
        var p3 = p2[1].Split(")");
        return (p1[0], p2[0], p3[0]);
        }
      ).ToList();

      var leftMap = nodes.ToDictionary(n => n.Item1, n => n.Item2);
      var rightMap = nodes.ToDictionary(n => n.Item1, n => n.Item3);

      var count = 0;
      var location = "AAA";

      while(true)
      foreach(var c in instructions)
      {
        var mapToUse = c == 'R' ? rightMap : leftMap;
        count++;
        location = mapToUse[location];

        if(location == "ZZZ")
        {
          Console.WriteLine(count);
          return;
        }
      }

    }

    public static void RunPart2()
    {

      var lines = File.ReadAllLines("./PuzzleInputDay8.txt").ToList();
      var instructions = lines[0];
      var nodes = lines.Skip(2).Select(l => {
        var p1 = l.Split(" = (");
        var p2 = p1[1].Split(", ");
        var p3 = p2[1].Split(")");
        return (p1[0], p2[0], p3[0]);
      }
      ).ToList();

      Console.WriteLine($"Instruction length {instructions.Length}");

      var leftMap = nodes.ToDictionary(n => n.Item1, n => n.Item2);
      var rightMap = nodes.ToDictionary(n => n.Item1, n => n.Item3);

      var distanceMap = new Dictionary<(string, long), MapValue>();
      var locationNodes = leftMap.Keys.Where(n => n.EndsWith('A')).Select(name => new MapKey { Node = name, InstructionIndex = 0 }).ToList();

      foreach(var node in locationNodes)
      {
        UpdateToNextEndNode(node, instructions, leftMap, rightMap);
        Console.WriteLine(node.InstructionIndex/269);
      }



      while(true)
      {
        if (locationNodes.Select(n => n.InstructionIndex).Distinct().Count() == 1)
        {
          Console.WriteLine(locationNodes[0].InstructionIndex);
          return;
        }


        var lowestNode = locationNodes.Aggregate((currentMinimum, node) => (currentMinimum == null || node.InstructionIndex < currentMinimum.InstructionIndex ? node : currentMinimum));
        //Console.WriteLine($"Searching for ({lowestNode.Node}, {lowestNode.InstructionIndex % instructions.Length})");
        if (distanceMap.TryGetValue((lowestNode.Node, lowestNode.InstructionIndex % instructions.Length), out var cachedValue))
        {
          //Console.WriteLine($"Found: {lowestNode.Node} -> {cachedValue.Node} : {lowestNode.InstructionIndex} -> {lowestNode.InstructionIndex + cachedValue.InstructionCount}");
          lowestNode.Node = cachedValue.Node;
          lowestNode.InstructionIndex += cachedValue.InstructionCount;
          continue;
        }

        var startingValues = new MapKey { InstructionIndex = lowestNode.InstructionIndex, Node = lowestNode.Node };
        UpdateToNextEndNode(lowestNode, instructions, leftMap, rightMap);

        distanceMap[(startingValues.Node, startingValues.InstructionIndex % instructions.Length)] = new MapValue { Node = lowestNode.Node, InstructionCount = lowestNode.InstructionIndex - startingValues.InstructionIndex };
        Console.WriteLine($"Adding ({startingValues.Node}, {startingValues.InstructionIndex % instructions.Length}) => ({lowestNode.Node}, {lowestNode.InstructionIndex - startingValues.InstructionIndex })");
        Console.WriteLine($"{startingValues.InstructionIndex}, {(lowestNode.InstructionIndex - startingValues.InstructionIndex) / 269}");

        //Console.WriteLine($"Entry added.  Index {lowestNode.InstructionIndex}.  Total size {distanceMap.Count}.");
      }
    }

    private static void UpdateToNextEndNode(MapKey node, string instructions, Dictionary<string, string> leftMap, Dictionary<string, string> rightMap)
    {
      while(true)
      {
        var instructionIndex = (int)node.InstructionIndex % instructions.Length;
        var mapToUse = instructions[instructionIndex] == 'R' ? rightMap : leftMap;
        node.InstructionIndex++;
        node.Node = mapToUse[node.Node];
        if(node.Node.EndsWith('Z'))
        {
          return;
        }
      }
    }

    public class MapKey : IEquatable<MapKey>
    {
      public string Node { get; set; }
      public long InstructionIndex { get; set; }

      public bool Equals([AllowNull] MapKey other)
      {
        return Node == other.Node && InstructionIndex == other.InstructionIndex;
      }
    }

    public class MapValue
    {
      public string Node { get; set; }
      public long InstructionCount { get; set; }
    }

  }
}
