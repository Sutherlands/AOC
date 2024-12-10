using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace _2024
{
  public static class Day10
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay10.txt").ToList();
      var grid = lines.Select(l => l.Select(c => int.Parse(c.ToString())).ToList()).ToList();

      var sum = 0;

      for (int x = 0; x < grid[0].Count; ++x)
      {
        for (int y = 0; y < grid.Count; ++y)
        {
          if (grid[y][x] == 0)
          {
            sum += GetReachabilityScore(grid, (x, y));
          }
        }
      }

      Console.WriteLine(sum);
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay10.txt").ToList();
      var grid = lines.Select(l => l.Select(c => int.Parse(c.ToString())).ToList()).ToList();

      var sum = 0;

      for (int x = 0; x < grid[0].Count; ++x)
      {
        for (int y = 0; y < grid.Count; ++y)
        {
          if (grid[y][x] == 0)
          {
            sum += GetRating(grid, (x, y), 0);
          }
        }
      }

      Console.WriteLine(sum);
    }

    private static int GetRating(List<List<int>> yxGrid, (int x, int y) targetPoint, int targetHeight)
    {
      if (targetPoint.x < 0 || targetPoint.y < 0 || targetPoint.x >= yxGrid[0].Count || targetPoint.y >= yxGrid.Count)
      {
        return 0;
      }

      if (yxGrid[targetPoint.y][targetPoint.x] != targetHeight)
      {
        return 0;
      }

      if (yxGrid[targetPoint.y][targetPoint.x] == 9)
      {
        return 1;
      }


      return GetRating(yxGrid, (targetPoint.x, targetPoint.y + 1), targetHeight + 1)
      + GetRating(yxGrid, (targetPoint.x, targetPoint.y - 1), targetHeight + 1)
      + GetRating(yxGrid, (targetPoint.x + 1, targetPoint.y), targetHeight + 1)
      + GetRating(yxGrid, (targetPoint.x - 1, targetPoint.y), targetHeight + 1);
    }

    public static int GetReachabilityScore(List<List<int>> yxGrid, (int x, int y) startingPosition)
    {
      var topLevelNodes = new HashSet<(int x, int y)>();
      var nodesToProcess = new Queue<(int x, int y)>();
      nodesToProcess.Enqueue(startingPosition);

      while (nodesToProcess.TryDequeue(out var node))
      {
        var height = yxGrid[node.y][node.x];
        if (height == 9)
        {
          topLevelNodes.Add(node);
        }

        AddIfValue(nodesToProcess, yxGrid, (node.x, node.y + 1), height + 1);
        AddIfValue(nodesToProcess, yxGrid, (node.x, node.y - 1), height + 1);
        AddIfValue(nodesToProcess, yxGrid, (node.x + 1, node.y), height + 1);
        AddIfValue(nodesToProcess, yxGrid, (node.x - 1, node.y), height + 1);
      }

      return topLevelNodes.Count;
    }

    private static void AddIfValue(Queue<(int x, int y)> nodesToProcess, List<List<int>> yxGrid, (int x, int y) targetPoint, int targetValue)
    {
      if (targetPoint.x < 0 || targetPoint.y < 0 || targetPoint.x >= yxGrid[0].Count || targetPoint.y >= yxGrid.Count)
      {
        return;
      }

      var value = yxGrid[targetPoint.y][targetPoint.x];
      if (value == targetValue)
      {
        nodesToProcess.Enqueue(targetPoint);
      }
    }
  }
}
