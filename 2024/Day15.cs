using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace _2024
{
  public static class Day15
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay15.txt").ToList();

      var grid = lines.TakeWhile(l => l != "").Select(l => l.ToList()).ToList();
      var instructions = lines.Last();


      foreach (var instruction in instructions)
      {
        (int x, int y) robotPosition = GetCurrentPosition(grid);
        PerformMove(robotPosition, instruction, grid);
        //Print(grid);
        //Console.ReadKey();
      }

      var boxSum = 0;
      for (int x = 0; x < grid[0].Count; ++x)
      {
        for (int y = 0; y < grid.Count; ++y)
        {
          if (grid[y][x] == 'O')
          {
            boxSum += y * 100 + x;
          }
        }
      }

      Console.WriteLine(boxSum);
    }

    private static void Print(List<List<char>> grid)
    {
      for (int y = 0; y < grid.Count; ++y)
      {
        for (int x = 0; x < grid[0].Count; ++x)
        {
          Console.Write(grid[y][x]);
        }
        Console.WriteLine();
      }
    }

    private static void PerformMove((int x, int y) currentPosition, char instruction, List<List<char>> grid)
    {
      var nextPosition = currentPosition;
      switch (instruction)
      {
        case '<':
          nextPosition.x--;
          break;
        case '>':
          nextPosition.x++;
          break;
        case '^':
          nextPosition.y--;
          break;
        case 'v':
          nextPosition.y++;
          break;
      }

      if (grid[nextPosition.y][nextPosition.x] == '#')
      {
        return;
      }

      if (grid[nextPosition.y][nextPosition.x] == 'O')
      {
        PerformMove(nextPosition, instruction, grid);
      }

      if (grid[nextPosition.y][nextPosition.x] == '.')
      {
        grid[nextPosition.y][nextPosition.x] = grid[currentPosition.y][currentPosition.x];
        grid[currentPosition.y][currentPosition.x] = '.';
      }
    }


    private static void PerformLargeMove((int x, int y) currentPosition, char instruction, List<List<char>> grid)
    {
      var nextPosition = currentPosition;
      switch (instruction)
      {
        case '<':
          nextPosition.x--;
          break;
        case '>':
          nextPosition.x++;
          break;
        case '^':
          nextPosition.y--;
          break;
        case 'v':
          nextPosition.y++;
          break;
      }

      if (grid[nextPosition.y][nextPosition.x] == '[' || grid[nextPosition.y][nextPosition.x] == ']')
      {
        if (new[] { '<', '>' }.Contains(instruction))
        {
          PerformLargeMove(nextPosition, instruction, grid);
        }
        else
        {
          var alternatePosition = (nextPosition.x + (grid[nextPosition.y][nextPosition.x] == '[' ? 1 : -1), nextPosition.y);
          PerformLargeMove(nextPosition, instruction, grid);
          PerformLargeMove(alternatePosition, instruction, grid);
        }
        PerformMove(nextPosition, instruction, grid);
      }

      if (grid[nextPosition.y][nextPosition.x] == '.')
      {
        grid[nextPosition.y][nextPosition.x] = grid[currentPosition.y][currentPosition.x];
        grid[currentPosition.y][currentPosition.x] = '.';
      }
    }

    private static bool CanDoLargeMove((int x, int y) currentPosition, char instruction, List<List<char>> grid)
    {
      var nextPosition = currentPosition;
      switch (instruction)
      {
        case '<':
          nextPosition.x--;
          break;
        case '>':
          nextPosition.x++;
          break;
        case '^':
          nextPosition.y--;
          break;
        case 'v':
          nextPosition.y++;
          break;
      }

      switch (grid[nextPosition.y][nextPosition.x])
      {
        case '#':
          return false;
        case '[':
        case ']':
          if (new[] { '<', '>' }.Contains(instruction))
            return CanDoLargeMove(nextPosition, instruction, grid);
          var alternatePosition = (nextPosition.x + (grid[nextPosition.y][nextPosition.x] == '[' ? 1 : -1), nextPosition.y);
          return CanDoLargeMove(nextPosition, instruction, grid) && CanDoLargeMove(alternatePosition, instruction, grid);
        case '.':
          return true;
        default:
          throw new Exception();
      }
    }


    private static (int x, int y) GetCurrentPosition(List<List<char>> grid)
    {
      for (int x = 0; x < grid[0].Count; ++x)
        for (int y = 0; y < grid.Count; ++y)
        {
          if (grid[y][x] == '@')
          {
            return (x, y);
          }
        }

      return (-1, -1);
    }

    private static IEnumerable<char> GetReplacements(char r)
    {
      switch (r)
      {
        case '#':
          yield return '#';
          yield return '#';
          break;
        case 'O':
          yield return '[';
          yield return ']';
          break;
        case '.':
          yield return '.';
          yield return '.';
          break;
        case '@':
          yield return '@';
          yield return '.';
          break;
        default:
          throw new Exception();
      }
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay15.txt").ToList();
      var grid = lines.TakeWhile(l => l != "").Select(l => l.SelectMany(GetReplacements).ToList()).ToList();
      var instructions = lines.SkipWhile(l => l.Length == 0 || !new[] { '<', '>', '^', 'v' }.Contains(l[0])).SelectMany(l => l).ToList();

      //Print(grid);
      foreach (var instruction in instructions)
      {
        (int x, int y) robotPosition = GetCurrentPosition(grid);
        if (CanDoLargeMove(robotPosition, instruction, grid))
          PerformLargeMove(robotPosition, instruction, grid);
        //Print(grid);
        //Console.ReadKey();
      }

      var boxSum = 0;
      for (int x = 0; x < grid[0].Count; ++x)
      {
        for (int y = 0; y < grid.Count; ++y)
        {
          if (grid[y][x] == '[')
          {
            boxSum += y * 100 + x;
          }
        }
      }

      Console.WriteLine(boxSum);
    }
  }
}
