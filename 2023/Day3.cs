using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace _2023
{
  public static class Day3
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay3.txt").ToList();

      if (lines.Any(l => l.Length != lines[0].Length))
      {
        Console.WriteLine("Anomaly detected");
        return;
      }

      var total = 0;
      for (int y = 0; y < lines.Count; ++y)
      {
        var currentNumber = 0;
        var partFound = false;

        for (int x = 0; x < lines[0].Length; ++x)
        {
          if (lines[y][x] >= '0' && lines[y][x] <= '9')
          {
            currentNumber = currentNumber * 10 + int.Parse(lines[y][x].ToString());
            partFound = partFound || FindPart(x, y, lines);
          }
          else
          {
            if (currentNumber != 0)
            {
              if (partFound)
              {
                Console.WriteLine($"Adding {currentNumber}");
                total += currentNumber;
              }
              else
              {
                Console.WriteLine($"Ignoring {currentNumber}");
              }
              currentNumber = 0;
              partFound = false;
            }
          }
        }

        if (currentNumber != 0)
        {
          if (partFound)
          {
            Console.WriteLine($"Adding {currentNumber}");
            total += currentNumber;
          }
          else
          {
            Console.WriteLine($"Ignoring {currentNumber}");
          }
          currentNumber = 0;
          partFound = false;
        }
      }
      Console.WriteLine(total);
    }

    private static bool FindPart(int x, int y, List<string> lines)
    {
      for (int xMod = -1; xMod <= 1; ++xMod)
      {
        for (int yMod = -1; yMod <= 1; ++yMod)
        {
          if (xMod == 0 && yMod == 0)
            continue;
          if (x + xMod < 0 || y + yMod < 0 || x + xMod >= lines[0].Length || y + yMod >= lines.Count)
            continue;
          var targetChar = lines[y + yMod][x + xMod];
          if (targetChar != '.' && (targetChar < '0' || targetChar > '9'))
          {
            return true;
          }
        }
      }
      return false;
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay3.txt").ToList();

      if (lines.Any(l => l.Length != lines[0].Length))
      {
        Console.WriteLine("Anomaly detected");
        return;
      }

      var total = 0;
      for (int y = 0; y < lines.Count; ++y)
      {
        for (int x = 0; x < lines[0].Length; ++x)
        {
          if (lines[y][x] == '*')
          {
            total += GetGearValue(x, y, lines);
          }
        }
      }

      Console.WriteLine(total);
    }

    private static int GetGearValue(int gearX, int gearY, List<string> lines)
    {
      var currentNumber = 0;
      var isNextTo = false;
      var totalAdjacent = 0;
      var gearRatio = 1;
      Console.WriteLine($"Processing gear at ({gearX},{gearY})");

      for (int y = Math.Max(gearY - 1, 0); y <= Math.Min(gearY + 1, lines.Count); ++y)
      {
        for (int x = 0; x < lines[0].Length; ++x)
        {
          if (lines[y][x] >= '0' && lines[y][x] <= '9')
          {
            currentNumber = currentNumber * 10 + int.Parse(lines[y][x].ToString());
            isNextTo = isNextTo || (Math.Abs(x - gearX) <= 1 && Math.Abs(y - gearY) <= 1);
          }
          else
          {
            if (currentNumber != 0)
            {
              if (isNextTo)
              {
                totalAdjacent++;
                gearRatio *= currentNumber;
              }
              currentNumber = 0;
              isNextTo = false;
            }
          }
        }

        if (currentNumber != 0)
        {
          if (isNextTo)
          {
            totalAdjacent++;
            gearRatio *= currentNumber;
          }
          currentNumber = 0;
          isNextTo = false;
        }
      }

      return totalAdjacent == 2 ? gearRatio : 0;
    }
  }
}
