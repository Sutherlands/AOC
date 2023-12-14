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
  public static class Day13
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay13.txt").Concat(new[] { "" }).ToList();
      var breaks = lines.Select((l, index) => (l, index)).Where(input => string.IsNullOrWhiteSpace(input.l)).Select(input => input.index).ToList();

      var puzzles = new List<List<string>>();

      for(int x = 0; x < breaks.Count; ++x)
      {
        var start = x == 0 ? 0 : breaks[x - 1] + 1;

        puzzles.Add(lines.Skip(start).Take(breaks[x] - start).ToList());
      }

      var lineReflectionIndices = puzzles.Select(p => GetLineReflections(p, 0)).ToList();
      Console.WriteLine(lineReflectionIndices.Sum(i => i.x + i.y * 100));
    }

    private static (int x, int y) GetLineReflections(List<string> puzzle, int smudgesAllowed = 0)
    {
      for(int x = 1; x < puzzle[0].Length; ++x)
      {
        var mismatchedPlaces = 0;
        for(int xMod = 0; xMod < x && x + xMod < puzzle[0].Length; ++xMod)
        {
          mismatchedPlaces += MismatchedColumnPlaces(x - xMod - 1, x + xMod, puzzle);
          if(mismatchedPlaces > smudgesAllowed)
          {
            break;
          }
        }

        if(mismatchedPlaces == smudgesAllowed)
        {
          return (x, 0);
        }
      }

      for(int y = 1; y < puzzle.Count; ++y)
      {
        var mismatchedPlaces = 0;
        for(int yMod = 0; yMod < y && y + yMod < puzzle.Count; ++yMod)
        {
          mismatchedPlaces += MismatchedRowPlaces(y - yMod - 1, y + yMod, puzzle);
          if (mismatchedPlaces > smudgesAllowed)
          {
            break;
          }
        }

        if (mismatchedPlaces == smudgesAllowed)
        {
          return (0, y);
        }
      }

      throw new Exception();
    }

    private static int MismatchedColumnPlaces(int xIndexFirst, int xIndexSecond, List<string> puzzle)
    {
      var mismatchedPlaces = 0;
      for (int y = 0; y < puzzle.Count; ++y)
      {
        if (puzzle[y][xIndexFirst] != puzzle[y][xIndexSecond])
        {
          mismatchedPlaces++;
        }
      }
      return mismatchedPlaces;
    }

    private static int MismatchedRowPlaces(int yIndexFirst, int yIndexSecond, List<string> puzzle)
    {
      var mismatchedPlaces = 0;
      for (int x = 0; x < puzzle[0].Length; ++x)
      {
        if (puzzle[yIndexFirst][x] != puzzle[yIndexSecond][x])
        {
          mismatchedPlaces++;
        }
      }
      return mismatchedPlaces;
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay13.txt").Concat(new[] { "" }).ToList();
      var breaks = lines.Select((l, index) => (l, index)).Where(input => string.IsNullOrWhiteSpace(input.l)).Select(input => input.index).ToList();

      var puzzles = new List<List<string>>();

      for (int x = 0; x < breaks.Count; ++x)
      {
        var start = x == 0 ? 0 : breaks[x - 1] + 1;

        puzzles.Add(lines.Skip(start).Take(breaks[x] - start).ToList());
      }

      var lineReflectionIndices = puzzles.Select(p => GetLineReflections(p, 1)).ToList();
      Console.WriteLine(lineReflectionIndices.Sum(i => i.x + i.y * 100));
    }
  }
}
