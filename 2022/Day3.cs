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

      var total = 0;
      foreach(var line in lines)
      {
        var length = line.Length/2;
        var firstPart = line.Take(length);
        var secondPart = line.Skip(length);
        var errorLetter = firstPart.Intersect(secondPart).Single();
        if(Char.IsUpper(errorLetter))
        {
          total += errorLetter - 'A' + 27;
        }
        else
        {
          total += errorLetter - 'a' + 1;
        }
      }
      Console.WriteLine(total);
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay3.txt").ToList();

      var total = 0;
      string line1 = null, line2 = null, line3 = null;
      foreach (var line in lines)
      {
        if (line1 == null)
        {
          line1 = line;
        }
        else if (line2 == null)
        {
          line2 = line;
        }
        else
        {
          line3 = line;

          var badgeLetter = line1.Intersect(line2).Intersect(line3).Single();
          if (Char.IsUpper(badgeLetter))
          {
            total += badgeLetter - 'A' + 27;
          }
          else
          {
            total += badgeLetter - 'a' + 1;
          }


          line1 = line2 = line3 = null;
        }
      }
      Console.WriteLine(total);
    }
  }
}
