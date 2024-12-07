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
  public static class Day6
  {
    enum Direction { North = 1, East = 2, South = 4, West = 8 }

    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay6.txt").Select(l => l.Replace('.', '\0').ToList()).ToList();
      var position = GetInitialPosition(lines);
      RunAlgorithm(lines, position);
      var count = lines.SelectMany(l => l).Count(c => c != '\0' && c != '#');
      Console.WriteLine(count);
    }

    private static bool RunAlgorithm(List<List<char>> lines, (int x, int y) position)
    {
      var direction = Direction.North;

      lines[position.y][position.x] = (char)direction;

      while (true)
      {
        var nextLocation = GetNextLocation(position, direction);
        if (nextLocation.x < 0 || nextLocation.y < 0 || nextLocation.x >= lines[0].Count || nextLocation.y >= lines.Count)
        {
          break;
        }

        if (lines[nextLocation.y][nextLocation.x] == '#')
        {
          switch (direction)
          {
            case Direction.North:
              direction = Direction.East;
              break;
            case Direction.East:
              direction = Direction.South;
              break;
            case Direction.South:
              direction = Direction.West;
              break;
            case Direction.West:
              direction = Direction.North;
              break;
          }
        }
        else if ((lines[nextLocation.y][nextLocation.x] & (char)direction) != 0)
        {
          return false;
        }
        else
        {
          position = nextLocation;
          lines[position.y][position.x] = (char)(lines[position.y][position.x] | (char)direction);
        }
      }

      return true;
    }

    private static (int x, int y) GetNextLocation((int x, int y) position, Direction direction)
    {
      switch (direction)
      {
        case Direction.North:
          return (position.x, position.y - 1);
        case Direction.East:
          return (position.x + 1, position.y);
        case Direction.South:
          return (position.x, position.y + 1);
        case Direction.West:
          return (position.x - 1, position.y);
        default:
          throw new Exception();
      }
    }

    private static (int x, int y) GetInitialPosition(List<List<char>> lines)
    {
      for (int y = 0; y < lines.Count; ++y)
      {
        for (int x = 0; x < lines[y].Count; ++x)
        {
          if (lines[y][x] == '^')
          {
            return (x, y);
          }
        }
      }

      throw new Exception();
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay6.txt").Select(l => l.Replace('.', '\0').ToList()).ToList();
      var position = GetInitialPosition(lines);
      var loopCount = 0;

      for(int x = 0; x < lines[0].Count; ++x)
      {
        for(int y = 0; y < lines.Count; ++y)
        {
          if(lines[y][x] != '\0')
          {
            continue;
          }

          var newLines = lines.Select(l => new List<char>(l)).ToList();
          newLines[y][x] = '#';
          if(!RunAlgorithm(newLines, position))
          {
            loopCount++;
          }
        }
      }

      Console.WriteLine(loopCount);
    }
  }
}
