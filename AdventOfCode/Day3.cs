using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
  public static class Day3
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay3.txt").ToList();
      var zeroCounts = new int[lines[0].Count()];
      var oneCounts = new int[lines[0].Count()];
      foreach (var line in lines)
      {
        for(int index = 0; index < line.Count(); ++index)
        {
          if(line[index] == '0')
          {
            zeroCounts[index]++;
          }
          else
          {
            oneCounts[index]++;
          }
        }
      }

      var gammaRate = 0;
      var epsilonRate = 0;
      for(int index = 0; index < zeroCounts.Count(); index++)
      {
        gammaRate *= 2;
        epsilonRate *= 2;
        if(oneCounts[index] > zeroCounts[index])
        {
          gammaRate += 1;
        }
        else
        {
          epsilonRate += 1;
        }
      }

      Console.WriteLine(gammaRate * epsilonRate);
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay3.txt").ToList();
      var oxygen = GetRating(lines, true);
      var co2 = GetRating(lines, false);
      Console.WriteLine(oxygen * co2);
    }

    public static int GetRating(List<string> data, bool useMostCommon)
    {
      var length = data[0].Length;
      for (int index = 0; index < length; ++index)
      {
        int ones = 0;
        int zeros = 0;
        foreach(var s in data)
        {
          if(s[index] == '1')
          {
            ones++;
          }
          else
          {
            zeros++;
          }
        }

        var appropriateNumber = ones < zeros ^ useMostCommon ? '1' : '0';
        data = data.Where(d => d[index] == appropriateNumber).ToList();

        if(data.Count() == 1)
        {
          return Convert.ToInt32(data[0], 2);
        }
      }
      throw new Exception();
    }
  }
}
