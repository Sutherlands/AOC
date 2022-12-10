using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
  public static class Day10
  {
    public static void RunPart1()
    {
      var sw = Stopwatch.StartNew();
      var lines = File.ReadAllLines("./PuzzleInputDay10.txt").ToList();

      var clock = 0;
      var register = 1;
      var signalStrengths = new List<int>();

      foreach(var line in lines)
      {
        var splits = line.Split(' ');
        var cycleLength = splits[0] == "noop" ? 1 : 2;
        if((clock+20)%40 > (clock+cycleLength+20)%40)
        {
          var passedClock = ((clock + cycleLength - 20) / 40) * 40 + 20;
          signalStrengths.Add(register * passedClock);
        }

        clock += cycleLength;
        if(cycleLength == 2)
        {
          register += int.Parse(splits[1]);
        }
      }


      Console.WriteLine(signalStrengths.Sum());
    }

    public static void RunPart2()
    {
      var sw = Stopwatch.StartNew();
      var lines = File.ReadAllLines("./PuzzleInputDay10.txt").ToList();

      var clock = 0;
      var register = 1;
      var signalStrengths = new List<int>();
      var pixels = new char[240];

      foreach (var line in lines)
      {
        var splits = line.Split(' ');
        var cycleLength = splits[0] == "noop" ? 1 : 2;
        if ((clock + 20) % 40 > (clock + cycleLength + 20) % 40)
        {
          var passedClock = ((clock + cycleLength - 20) / 40) * 40 + 20;
          signalStrengths.Add(register * passedClock);
        }

        pixels[clock] = IsLit(clock, register);
        if(cycleLength == 2)
        {
          pixels[clock + 1] = IsLit(clock+1, register);
        }


        clock += cycleLength;
        if (cycleLength == 2)
        {
          register += int.Parse(splits[1]);
        }
      }


      for(int x = 0; x < 240; ++x)
      {
        Console.Write(pixels[x]);
        if(x % 40 == 39)
        {
          Console.WriteLine();
        }

      }
    }

    public static char IsLit(int clock, int register)
    {
      return Math.Abs((clock % 40) - register) <= 1 ? '#' : '.';
    }

  }
}
