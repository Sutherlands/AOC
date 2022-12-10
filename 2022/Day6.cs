using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
  public static class Day6

  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay6.txt").ToList();

      var index = lines.First().Select((c, i) =>
      {
        if (lines[0].Skip(i).Take(14).Distinct().Count() == 14)
          return i;
        return -1;
      })
        .Where(i => i != -1)
        .First();
      Console.WriteLine(index+14);
    }

    public static void RunPart2()
    {
    }
  }
}
