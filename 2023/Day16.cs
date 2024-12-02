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
  public static class Day16
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay16.txt").ToList();
      var energizedMap = new bool[lines.Count][];
      var alreadyProcessedMap = new List<(Direction, int, int)>();
      for(int y = 0; y < lines.Count; ++y)
      {
        energizedMap[y] = new bool[lines[0].Length];
      }

      var queue = new Queue<(Direction direction, int x, int y)>();
      queue.Enqueue((Direction.East, 0, 0));

      while(queue.TryDequeue(out var toProcess))
      {
        ProcessLight(toProcess.x, toProcess.y, toProcess.direction, lines, energizedMap, alreadyProcessedMap, queue);
      }

      Console.WriteLine(energizedMap.SelectMany(m => m).Sum(m => m ? 1 : 0));
    }

    private static void ProcessLight(int x, int y, Direction direction, List<string> lines, bool[][] energizedMap, List<(Direction, int, int)> alreadyProcessedMap, Queue<(Direction direction, int x, int y)> queue)
    {
      if(x < 0 || y < 0 || x >= lines[0].Length || y >= lines.Count)
      {
        return;
      }

      if(alreadyProcessedMap.Contains((direction, x, y)))
      {
        return;
      }
      alreadyProcessedMap.Add((direction, x, y));

      energizedMap[y][x] = true;
      switch (lines[y][x])
      {
        case '.':
          queue.Enqueue((direction, x + GetDirectionModifier(true, direction), y + GetDirectionModifier(false, direction)));
          break;
        case '/':
        case '\\':
          direction = GetNewDirection(direction, lines[y][x]);
          queue.Enqueue((direction, x + GetDirectionModifier(true, direction), y + GetDirectionModifier(false, direction)));
          break;
        case '|':
          switch(direction)
          {
            case Direction.North:
            case Direction.South:
              queue.Enqueue((direction, x + GetDirectionModifier(true, direction), y + GetDirectionModifier(false, direction)));
              break;
            case Direction.East:
            case Direction.West:
              direction = Direction.North;
              queue.Enqueue((direction, x + GetDirectionModifier(true, direction), y + GetDirectionModifier(false, direction)));
              direction = Direction.South;
              queue.Enqueue((direction, x + GetDirectionModifier(true, direction), y + GetDirectionModifier(false, direction)));
              break;
            default:
              throw new NotImplementedException();
          }
          break;
        case '-':
          switch (direction)
          {
            case Direction.East:
            case Direction.West:
              queue.Enqueue((direction, x + GetDirectionModifier(true, direction), y + GetDirectionModifier(false, direction)));
              break;
            case Direction.North:
            case Direction.South:
              direction = Direction.East;
              queue.Enqueue((direction, x + GetDirectionModifier(true, direction), y + GetDirectionModifier(false, direction)));
              direction = Direction.West;
              queue.Enqueue((direction, x + GetDirectionModifier(true, direction), y + GetDirectionModifier(false, direction)));
              break;
            default:
              throw new NotImplementedException();
          }
          break;
      }
    }

    private static Direction GetNewDirection(Direction direction, char mirror)
    {
      switch (mirror)
      {
        case '/':
          switch (direction)
          {
            case Direction.East:
              return Direction.North;
            case Direction.West:
              return Direction.South;
            case Direction.North:
              return Direction.East;
            case Direction.South:
              return Direction.West;
          }
          throw new NotImplementedException();
        case '\\':
          switch (direction)
          {
            case Direction.East:
              return Direction.South;
            case Direction.West:
              return Direction.North;
            case Direction.North:
              return Direction.West;
            case Direction.South:
              return Direction.East;
          }
          throw new NotImplementedException();
      }
      throw new NotImplementedException();
    }

    public enum Direction
    {
      North, South, East, West
    }

    public static int GetDirectionModifier(bool isX, Direction direction)
    {
      if (isX && direction == Direction.East)
      {
        return 1;
      }

      if (isX && direction == Direction.West)
      {
        return -1;
      }

      if (!isX && direction == Direction.North)
      {
        return -1;
      }

      if (!isX && direction == Direction.South)
      {
        return 1;
      }

      return 0;
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay16.txt").ToList();

      var maxCount = 0;

      for(int x = 0; x < lines[0].Length; ++x)
      {
        Console.WriteLine(x);
        var count = GetEnergized(Direction.South, x, 0, lines);
        maxCount = Math.Max(count, maxCount);
        count = GetEnergized(Direction.North, x, lines.Count-1, lines);
        maxCount = Math.Max(count, maxCount);
      }

      for (int y = 0; y < lines.Count; ++y)
      {
        Console.WriteLine(y);
        var count = GetEnergized(Direction.East, 0, y, lines);
        maxCount = Math.Max(count, maxCount);
        count = GetEnergized(Direction.West, lines[0].Length - 1, y, lines);
        maxCount = Math.Max(count, maxCount);
      }

      Console.WriteLine(maxCount);
    }

    private static int GetEnergized(Direction direction, int startX, int startY, List<string> lines)
    {
      var energizedMap = new bool[lines.Count][];
      var alreadyProcessedMap = new List<(Direction, int, int)>();
      for (int y = 0; y < lines.Count; ++y)
      {
        energizedMap[y] = new bool[lines[0].Length];
      }

      var queue = new Queue<(Direction direction, int x, int y)>();
      queue.Enqueue((direction, startX, startY));

      while (queue.TryDequeue(out var toProcess))
      {
        ProcessLight(toProcess.x, toProcess.y, toProcess.direction, lines, energizedMap, alreadyProcessedMap, queue);
      }

      return energizedMap.SelectMany(m => m).Sum(m => m ? 1 : 0);
    }
  }
}
