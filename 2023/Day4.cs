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
  public static class Day4
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay4.txt").ToList();
      var total = 0;

      foreach(var line in lines)
      {
        var parts = line.Split(new char[] { ':','|' });
        var myNumbers = parts[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
        var winningNumbers = parts[2].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

        var winningNumberCount = myNumbers.Intersect(winningNumbers).Count();
        if(winningNumberCount > 0)
        {
          total += (int)Math.Pow(2, winningNumberCount - 1);
        }
      }
      Console.WriteLine(total);
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay4.txt").ToList();
      var winningNumberMap = new Dictionary<int, int>();

      foreach (var line in lines)
      {
        var parts = line.Split(new char[] { ':', '|' });
        var cardNumber = int.Parse(parts[0].Split(' ').Last())-1;
        var myNumbers = parts[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
        var winningNumbers = parts[2].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

        var winningNumberCount = myNumbers.Intersect(winningNumbers).Count();
        winningNumberMap[cardNumber] = winningNumberCount;
      }

      var totalWinning = new Dictionary<int, int>();
      for(int x = winningNumberMap.Count - 1; x >= 0; --x)
      {
        var winningNumberCount = winningNumberMap[x];
        var totalWinningCount = Enumerable.Range(x + 1, winningNumberCount).Sum(n => totalWinning[n]) + 1;
        totalWinning[x] = totalWinningCount;
      }

      Console.WriteLine(totalWinning.Values.Sum());

    }
  }
}
